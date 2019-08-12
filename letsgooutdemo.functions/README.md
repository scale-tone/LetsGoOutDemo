# LetsGoOutDemo.Functions

Implements the Appointments Saga with [Azure Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview).
Sends appointment state change notifications to web clients via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview), thus demonstrating how to use [SignalR](http://signalr.net) in Serverless scenarios.
The actual code is [here](https://github.com/scale-tone/LetsGoOutDemo/blob/master/letsgooutdemo.functions/LetsGoOutSaga.cs).