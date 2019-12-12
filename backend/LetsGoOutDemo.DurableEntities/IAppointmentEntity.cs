using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsGoOutDemo.DurableEntities
{
    // Enum representing the appointment status
    public enum AppointmentStatusEnum
    {
        Pending = 0,
        Accepted,
        Declined
    }

    // Internal representation of participant's response
    public class AppointmentResponse
    {
        public string NickName { get; set; }
        public bool IsAccepted { get; set; }
    }

    // Interface of a Durable Entity representing the negotiated appointment
    public interface IAppointmentEntity
    {
        // Initializes an appointment with the list of participants
        Task InitializeParticipantsAsync(HashSet<string> participants);

        // Accepts a response from a participant
        Task SetResponseAsync(AppointmentResponse response);
    }
}