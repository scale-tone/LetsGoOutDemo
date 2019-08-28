# LetsGoOutDemo

This demo project demonstrates various ways of implementing the Saga/Workflow design pattern in Azure.

The use case is the same and it is pretty basic: we build a stateful service, that handles organizing appointments with multiple participants. The participants communicate with the service via a client web UI and can submit appointment proposals, accept and/or decline them. The service takes care of maintaining the appointment state and notifies participants about state changes.

* [**LetsGoOutDemo.React**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/frontend/letsgooutdemo.react#letsgooutdemoreact) - a React+MobX+TypeScript simple web UI, that communicates with the backend and with [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview). This frontend can work with any of the below backend implementations.

* [**LetsGoOutDemo.Functions**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/LetsGoOutDemo.Functions#letsgooutdemofunctions) - backend implemented with Azure Durable Functions (C#). Appointment state change notifications are sent via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).

* [**LetsGoOutDemo.ServiceFabric**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/LetsGoOutDemo.ServiceFabric#letsgooutdemoservicefabric) - backend implemented with [Service Fabric Reliable Actors](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-actors-introduction). Appointment state change notifications are also sent via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).

* [**LetsGoOutDemo.AspNetCore**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/LetsGoOutDemo.AspNetCore#letsgooutdemoaspnetcore) - backend implemented with pure [Asp.Net Core](https://github.com/aspnet/AspNetCore) and [Redis Cache](https://redis.io). Appointment state change notifications are as well sent via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).
