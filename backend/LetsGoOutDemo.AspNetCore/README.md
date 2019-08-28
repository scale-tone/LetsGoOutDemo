# LetsGoOutDemo.AspNetCore

Implements the Appointments negotiation process with [Asp.Net Core](https://github.com/aspnet/AspNetCore) and [Redis Cache](https://redis.io).
Sends appointment state change notifications to web clients via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).

## How to run

* [Get yourself an Azure SignalR Service instance](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core#create-an-azure-signalr-resource).
* [Get yourself an Azure Cache for Redis instance](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/) or just [install Redis locally](https://www.nuget.org/packages/Redis-64/).
* Put your Azure SignalR Service Connection String and Redis Connection String into [here] (https://github.com/scale-tone/LetsGoOutDemo/blob/master/backend/LetsGoOutDemo.AspNetCore/LetsGoOutDemo.AspNetCore/appsettings.json#L9).
* Press F5.

The backend endpoint will become available at http://localhost:7071/api. [UI statics](https://github.com/scale-tone/LetsGoOutDemo/tree/master/frontend/letsgooutdemo.react#letsgooutdemoreact) are committed to the repo and are served by the endpoint itself, so you don't necessarily need to compile them. Just navigate to http://localhost:7071/api/ui.
