using Microsoft.AspNetCore.SignalR;

namespace LetsGoOutDemo.AspNetCore
{
    /// <summary>
    /// Provides SignalR with a userId upon negotiating a client connection.
    /// By default, SignalR tries to use CurrentPrincipal.Identity.Name for that,
    /// but since we do not have any proper authentication here implemented, we need 
    /// to help SignalR a bit.
    /// </summary>
    public class NickNameUserIdProvider: IUserIdProvider
    {
        /// <summary>
        /// Uses query string 'nick-name' parameter as a userId for SignalR connection.
        /// </summary>
        public string GetUserId(HubConnectionContext connection)
        {
            string nickName = connection.GetHttpContext().Request.Query["nick-name"];
            return nickName;
        }
    }
}
