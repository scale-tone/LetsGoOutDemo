
namespace LetsGoOutDemo.ServiceFabric.Actors.Interfaces
{
    /// <summary>
    /// Enum representing the appointment status
    /// </summary>
    public enum AppointmentStatusEnum
    {
        Pending = 0,
        Accepted,
        Declined
    }

    public class Constants
    {
        public const string AzureSignalRConnectionStringEnvironmentVariableName = "AzureSignalRConnectionString";
        public const string AllowedCorsOriginsVariableName = "AllowedCorsOrigins";
    }
}
