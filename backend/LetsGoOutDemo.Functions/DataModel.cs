using System.Collections.Generic;

namespace LetsGoOutDemo.Functions
{
    // Enum representing the appointment status
    public enum AppointmentStatusEnum
    {
        Pending = 0,
        Accepted,
        Declined
    }

    // Internal representation of an appointment
    public class Appointment
    {
        // The original list of participants
        public HashSet<string> Participants { get; set; }
        // The list of participants that have accepted the appointment
        public HashSet<string> ParticipantsAccepted { get; set; }
        // The current status of the appointment
        public AppointmentStatusEnum Status { get; set; }

        // Is the appointment accepted by all participants?
        public bool HasEveryoneAccepted()
        {
            return this.Participants.SetEquals(this.ParticipantsAccepted);
        }
    }

    // Internal representation of participant's response
    public class AppointmentResponse
    {
        public string NickName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
