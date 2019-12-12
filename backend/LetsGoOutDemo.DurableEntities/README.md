# LetsGoOutDemo.DurableEntities

Implements the appointment negotiation process with [Azure Durable Entities](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-entities).
Sends appointment state change notifications to web clients via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview), thus demonstrating how to use [SignalR](http://signalr.net) in Serverless scenarios.

## Prerequisites

[Azure Functions Core Tools](https://www.npmjs.com/package/azure-functions-core-tools).

## How to run

* [Get yourself an Azure SignalR Service instance](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core#create-an-azure-signalr-resource). NOTE: the [**Service Mode** setting shoud be set to **Serverless**](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-azure-functions-csharp)!
* In the project root folder create a **local.settings.json** file, that looks like this:

``
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "[your Azure Storage Account Connection String]",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "AzureSignalRConnectionString": "[your Azure SignalR Connection String]"
    },
    "Host": {
        "LocalHttpPort": 7071,
        "CORS": "http://localhost:3000",
        "CORSCredentials": true
    }
}
``

* **func start**

This will compile and run the Functions project on your local devbox under http://localhost:7071/api.
[UI statics](https://github.com/scale-tone/LetsGoOutDemo/tree/master/frontend/letsgooutdemo.react#letsgooutdemoreact) are committed to the repo and are served by the Function, so you don't necessarily need to compile them. Just navigate to http://localhost:7071/api/ui.