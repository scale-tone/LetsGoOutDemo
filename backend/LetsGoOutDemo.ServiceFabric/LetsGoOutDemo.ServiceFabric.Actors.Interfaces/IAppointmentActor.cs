using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace LetsGoOutDemo.ServiceFabric.Actors.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IAppointmentActor : IActor
    {
        /// <summary>
        /// Initializes an appointment with the list of participants
        /// </summary>
        Task InitializeParticipantsAsync(HashSet<string> participants);

        /// <summary>
        /// Accepts a response from a participant
        /// </summary>
        Task SetResponseAsync(string participant, bool isAccepted);
    }
}
