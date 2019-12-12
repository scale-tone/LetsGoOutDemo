using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Linq;

namespace LetsGoOutDemo.DurableEntities
{
    // A Durable Entity representing the negotiated appointment
    public class AppointmentEntity : IAppointmentEntity
    {
        // The below two are the actual Entity Status. Must be serializable.
        public HashSet<string> Participants { get; set; }
        public HashSet<string> ParticipantsAccepted { get; set; }

        // An object to output SignalR messages to. Passed via input binding.
        private readonly IAsyncCollector<SignalRMessage> signalRCollector;

        // ctor
        public AppointmentEntity(IAsyncCollector<SignalRMessage> signalRCollector)
        {
            this.signalRCollector = signalRCollector;
            this.Participants = new HashSet<string>();
            this.ParticipantsAccepted = new HashSet<string>();
        }

        // Initializes an appointment with the list of participants
        public async Task InitializeParticipantsAsync(HashSet<string> participants)
        {
            this.Participants = participants;
            await this.NotifyParticipants(this.Participants.ToArray(), AppointmentStatusEnum.Pending);
        }

        // Accepts a response from a participant
        public async Task SetResponseAsync(AppointmentResponse response)
        {
            // Just to make sure this response came from one of participants
            if(!this.Participants.Contains(response.NickName))
            {
                return;
            }

            // If someone has rejected...
            if (!response.IsAccepted)
            {
                // ... then notifying everybody and killing ourselves
                await this.NotifyParticipants(this.Participants.ToArray(), AppointmentStatusEnum.Declined);
                Entity.Current.DeleteState();
            }

            this.ParticipantsAccepted.Add(response.NickName);

            // If everybody have accepted
            if(this.Participants.SetEquals(this.ParticipantsAccepted))
            {
                // ... then notifying everybody and killing ourselves
                await this.NotifyParticipants(this.Participants.ToArray(), AppointmentStatusEnum.Accepted);
                Entity.Current.DeleteState();
            }
        }

        // Boilerplate for running this Durable Entity
        [FunctionName(nameof(AppointmentEntity))]
        public static Task Run(
            [EntityTrigger] IDurableEntityContext context,
            [SignalR(HubName = Functions.SignalRHubName)] IAsyncCollector<SignalRMessage> signalRCollector
        )
        {
            return context.DispatchAsync<AppointmentEntity>(signalRCollector);
        }

        // Informs participants of the appointment state changes via SignalR
        private async Task NotifyParticipants(string[] nickNames, AppointmentStatusEnum status)
        {
            string appointmentId = Entity.Current.EntityKey;

            foreach (string nickName in nickNames)
            {
                var signalRMessage = new SignalRMessage
                {
                    UserId = nickName,
                    Target = Functions.SignalRClientHandlerName,
                    Arguments = new[]
                        {
                            new
                            {
                                id = appointmentId,
                                participants = nickNames,
                                status
                            }
                        }
                };
                await signalRCollector.AddAsync(signalRMessage);
            }
        }
    }
}