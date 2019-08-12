using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Collections.Generic;
using System.Linq;

namespace LetsGoOutDemo.Functions
{
    // Durable Function, that handles the appointment negotiation process
    public static class LetsGoOutSaga
    {
        private const string SignalRHubName = "LetsGoOutHub";
        private const string NickNameHeaderName = "x-nick-name";

        // SignalR connection negotiation method. The name is predefined.
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] 
                HttpRequest request,
            [SignalRConnectionInfo(HubName = SignalRHubName, UserId = "{query.nick-name}")] 
                SignalRConnectionInfo connectionInfo)
        {
            // Simply returning service instance URL and access token (it will be tied to this particular nickName)
            return connectionInfo;
        }

        // Creates a new Appointment Saga
        [FunctionName("new-appointment")]
        public static async Task<IActionResult> Invite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
            [OrchestrationClient] DurableOrchestrationClient orchestrationClient,
            ILogger log)
        {
            var nickNames = (await request.ReadAsStringAsync())
                .Split(',')
                .Select(n => n.Trim())
                .ToList();

            string initiatorNickName = request.Headers[NickNameHeaderName];
            if(string.IsNullOrEmpty(initiatorNickName))
            {
                // The initiator's nickName should always be send via that header
                return new UnauthorizedResult();
            }

            // Also adding the initiator to the list
            nickNames.Add(initiatorNickName);

            // Here is where the Saga instance ID is being generated.
            // Some prefix is needed before a sortable datetime, otherwise SignalR treats this value as a datetime 
            // (not as a string) somewhere inside and trims trailing zeroes from it, effectively corrupting this ID.
            string appointmentId = "APP-" + DateTime.UtcNow.ToString("o");

            // Starting the appointment Saga
            await orchestrationClient.StartNewAsync(nameof(ProcessAppointmentOrchestrator), 
                appointmentId, 
                new Appointment
                {
                    Participants = new HashSet<string>(nickNames),
                    ParticipantsAccepted = new HashSet<string>()
                });

            return new OkResult();
        }

        // Saga Orchestration method. The Saga logic is concentrated here.
        [FunctionName(nameof(ProcessAppointmentOrchestrator))]
        public static async Task ProcessAppointmentOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log)
        {
            string appointmentId = context.InstanceId;
            var appointment = context.GetInput<Appointment>();

            // Retry policy for calling our activity functions
            var retryOptions = new RetryOptions(TimeSpan.FromSeconds(2), 3);

            // Notifying all participants that a new appointment was created
            await context.CallActivityWithRetryAsync<Appointment>(nameof(NotifyParticipants), retryOptions, appointment);

            // Now waiting for responses
            while(true)
            {
                var response = await context.WaitForExternalEvent<AppointmentResponse>(nameof(RespondToAppointment));

                // Just to make sure this response came from one of participants
                if(!appointment.Participants.Contains(response.NickName))
                {
                    continue;
                }

                // If someone has rejected...
                if(!response.IsAccepted)
                {
                    // ... then notifying everybody and finishing the Saga
                    appointment.Status = AppointmentStatusEnum.Declined;
                    await context.CallActivityWithRetryAsync<Appointment>(nameof(NotifyParticipants), retryOptions, appointment);
                    break;
                }

                appointment.ParticipantsAccepted.Add(response.NickName);

                // If everybody have accepted
                if(appointment.HasEveryoneAccepted())
                {
                    // ... then notifying everybody and finishing the Saga
                    appointment.Status = AppointmentStatusEnum.Accepted;
                    await context.CallActivityWithRetryAsync<Appointment>(nameof(NotifyParticipants), retryOptions, appointment);
                    break;
                }
            }
        }

        // Activity function, that informs participants of the appointment state changes
        [FunctionName(nameof(NotifyParticipants))]
        public static async Task NotifyParticipants(
            [ActivityTrigger] DurableActivityContext context,
            [SignalR(HubName = SignalRHubName)] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string appointmentId = context.InstanceId;
            var appointment = context.GetInput<Appointment>();
            var nickNames = appointment.Participants.ToArray();

            foreach (string nickName in nickNames)
            {
                var signalRMessage = new SignalRMessage
                {
                    UserId = nickName,
                    Target = "appointment-state-changed",
                    Arguments = new[]
                        {
                            new
                            {
                                id = appointmentId,
                                participants = nickNames,
                                status = appointment.Status
                            }
                        }
                };
                await signalRMessages.AddAsync(signalRMessage);
            }
        }

        // Receives responses from participants
        [FunctionName("appointments")]
        public static async Task<IActionResult> RespondToAppointment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "appointments/{appointmentId}")] HttpRequest request,            
            string appointmentId,
            [OrchestrationClient] DurableOrchestrationClient orchestrationClient,
            ILogger log)
        {
            var status = Enum.Parse<AppointmentStatusEnum>(await request.ReadAsStringAsync());
            if (status == AppointmentStatusEnum.Pending)
            {
                return new BadRequestResult();
            }

            // Transforming client's response into an Event
            var response = new AppointmentResponse
            {
                NickName = request.Headers[NickNameHeaderName],
                IsAccepted = (status == AppointmentStatusEnum.Accepted)
            };
            await orchestrationClient.RaiseEventAsync(appointmentId, nameof(RespondToAppointment), response);

            return new OkResult();
        }
    }
}