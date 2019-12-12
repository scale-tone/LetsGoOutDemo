using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace LetsGoOutDemo.DurableEntities
{
    // Exposes an HTTP endpoint for negotiating appointments
    public static class Functions
    {
        public const string SignalRHubName = "LetsGoOutHub";
        public const string NickNameHeaderName = "x-nick-name";
        public const string SignalRClientHandlerName = "appointment-state-changed";

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

        // Creates a new Appointment
        [FunctionName("new-appointment")]
        public static async Task<IActionResult> Invite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
            [DurableClient] IDurableClient durableClient,
            ILogger log)
        {
            var nickNames = (await request.ReadAsStringAsync())
                .Split(',')
                .Select(n => n.Trim())
                .ToList();

            string initiatorNickName = request.Headers[NickNameHeaderName];
            if (string.IsNullOrEmpty(initiatorNickName))
            {
                // The initiator's nickName should always be send via that header
                return new UnauthorizedResult();
            }

            // Also adding the initiator to the list
            nickNames.Add(initiatorNickName);

            // Here is where the AppointmentEntity Key is being generated.
            // Some prefix is needed before a sortable datetime, otherwise SignalR treats this value as a datetime 
            // (not as a string) somewhere inside and trims trailing zeroes from it, effectively corrupting this ID.
            string appointmentId = "APP-" + DateTime.UtcNow.ToString("o");

            // Here a new AppointmentEntity instance will be created and its InitializeParticipantsAsync() method will be called
            await durableClient.SignalEntityAsync<IAppointmentEntity>(appointmentId, 
                p => p.InitializeParticipantsAsync(new HashSet<string>(nickNames)));

            return new OkResult();
        }

        // Receives responses from participants
        [FunctionName("appointments")]
        public static async Task<IActionResult> RespondToAppointment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "appointments/{appointmentId}")] HttpRequest request,
            string appointmentId,
            [DurableClient] IDurableClient durableClient,
            ILogger log)
        {            
            var status = Enum.Parse<AppointmentStatusEnum>(await request.ReadAsStringAsync());
            if (status == AppointmentStatusEnum.Pending)
            {
                return new BadRequestResult();
            }

            // Transforming client's response into a Signal
            var response = new AppointmentResponse
            {
                NickName = request.Headers[NickNameHeaderName],
                IsAccepted = (status == AppointmentStatusEnum.Accepted)
            };
            await durableClient.SignalEntityAsync<IAppointmentEntity>(appointmentId, p => p.SetResponseAsync(response));

            return new OkResult();
        }
    }
}