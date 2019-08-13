using Microsoft.AspNetCore.SignalR;

namespace LetsGoOutDemo.ServiceFabric.Actors.Interfaces
{
    /// <summary>
    /// This class is only used to specify the Hub name. Clients are not calling its methods.
    /// </summary>
    public class LetsGoOutHub : Hub
    {
    }
}
