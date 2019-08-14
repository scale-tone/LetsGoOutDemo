# LetsGoOutDemo.ServiceFabric

Implements the appointment management with [Service Fabric Reliable Actors](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-actors-introduction).

## Prerequisites

[Microsoft Azure Service Fabric SDK](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started).

## How to run

* [Get yourself an Azure SignalR Service instance](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core#create-an-azure-signalr-resource).
* Put your Azure SignalR Connection String [into here](https://github.com/scale-tone/LetsGoOutDemo/blob/050cfefb83c2c66940043ea6cadcc38b46393584/backend/LetsGoOutDemo.ServiceFabric/LetsGoOutDemo.ServiceFabric/ApplicationParameters/Local.5Node.xml#L7).
* Press F5 in your Visual Studio.
* Wait until the backend becomes available under http://localhost:7071/api URL.

## Details

The solution contains two projects (both being deployed as services into your Service Fabric cluster):
* [LetsGoOutDemo.ServiceFabric.Actors](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/LetsGoOutDemo.ServiceFabric/LetsGoOutDemo.ServiceFabric.Actors) - here is where the [AppointmentActor](https://github.com/scale-tone/LetsGoOutDemo/blob/master/backend/LetsGoOutDemo.ServiceFabric/LetsGoOutDemo.ServiceFabric.Actors/AppointmentActor.cs) is implemented.
* [LetsGoOutDemo.ServiceFabric.WebApi](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/LetsGoOutDemo.ServiceFabric/LetsGoOutDemo.ServiceFabric.WebApi) - an ASP.Net Core web service, that [exposes the actor's functionality to the outer world](https://github.com/scale-tone/LetsGoOutDemo/blob/master/backend/LetsGoOutDemo.ServiceFabric/LetsGoOutDemo.ServiceFabric.WebApi/Controllers/AppointmentsController.cs). It also implements SignalR connection negotiation for the clients.
