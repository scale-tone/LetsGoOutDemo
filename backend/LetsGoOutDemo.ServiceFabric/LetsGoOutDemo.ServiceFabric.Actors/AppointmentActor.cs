using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using LetsGoOutDemo.ServiceFabric.Actors.Interfaces;
using Microsoft.Azure.SignalR.Management;

namespace LetsGoOutDemo.ServiceFabric.Actors
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class AppointmentActor : Actor, IAppointmentActor
    {
        private const string ParticipantsState = nameof(ParticipantsState);
        private const string ParticipantsAcceptedState = nameof(ParticipantsAcceptedState);
        private const string AppointmentStateChangedClientHandlerName = "appointment-state-changed";

        /// <summary>
        /// Initializes a new instance of AppointmentActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public AppointmentActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <inheritdoc/>
        public async Task InitializeParticipantsAsync(HashSet<string> participants)
        {
            await this.StateManager.AddStateAsync(ParticipantsState, participants);
            await this.StateManager.AddStateAsync(ParticipantsAcceptedState, new HashSet<string>());

            await this.NotifyParticipantsAsync(participants.ToArray(), AppointmentStatusEnum.Pending);
        }

        /// <inheritdoc/>
        public async Task SetResponseAsync(string participant, bool isAccepted)
        {
            var participants = await this.StateManager.GetStateAsync<HashSet<string>>(ParticipantsState);

            // Just to make sure this response came from one of participants
            if (!participants.Contains(participant))
            {
                return;
            }

            // If someone has rejected...
            if (!isAccepted)
            {
                await this.NotifyParticipantsAsync(participants.ToArray(), AppointmentStatusEnum.Declined);
                // This effectively terminates the workflow, so no need to save the changed state.
                // In real scenario though, you would want to persist the fact that the appointment was rejected,
                // to protect from participants who accidentally missed the notification.
                return;
            }

            var participantsAccepted = await this.StateManager.GetStateAsync<HashSet<string>>(ParticipantsAcceptedState);
            participantsAccepted.Add(participant);

            // If everybody have accepted
            if (participants.SetEquals(participantsAccepted))
            {
                await this.NotifyParticipantsAsync(participants.ToArray(), AppointmentStatusEnum.Accepted);
            }

            // Saving the changed state
            await this.StateManager.SetStateAsync(ParticipantsAcceptedState, participantsAccepted);
        }

        /// <summary>
        /// Sends SignalR change notifications to all participants of the appointment
        /// </summary>
        private async Task NotifyParticipantsAsync(string[] nickNames, AppointmentStatusEnum status)
        {
            var hubContext = HubContextTask.Result;

            // This actor's ID is the appointmentId as well
            var appointmentId = this.Id.GetGuidId();

            foreach (string nickName in nickNames)
            {
                await hubContext.Clients.User(nickName).SendCoreAsync(AppointmentStateChangedClientHandlerName,
                    new[]
                    {
                        new
                        {
                            id = appointmentId,
                            participants = nickNames,
                            status
                        }
                    }
                );
            }
        }

        /// <summary>
        /// A singleton instance of IServiceHubContext.
        /// A task of creating it is triggered by static ctor, and then the created instance is reused
        /// by all actor instances. IServiceHubContext is declared to be thread-safe.
        /// </summary>
        private static readonly Task<IServiceHubContext> HubContextTask = CreateHubContext();

        private static Task<IServiceHubContext> CreateHubContext()
        {
            string signalRConnString = Environment.GetEnvironmentVariable(Constants.AzureSignalRConnectionStringEnvironmentVariableName);

            var serviceManager = new ServiceManagerBuilder()
                .WithOptions(option =>
                {
                    option.ConnectionString = signalRConnString;
                    // ServiceTransportType.Persistent would be more efficient, but it is not reliable (connection might be dropped and never reestablished)
                    option.ServiceTransportType = ServiceTransportType.Transient;
                })
                .Build();

            return serviceManager.CreateHubContextAsync(nameof(LetsGoOutHub));
        }
    }
}
