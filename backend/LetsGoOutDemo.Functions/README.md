<img src="https://dev.azure.com/kolepes/LetsGoOutDemo/_apis/build/status/LetsGoOutDemo-Azure%20Functions%20for%20.NET-CI"/>

# LetsGoOutDemo.Functions

Implements the Appointments Saga with [Azure Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview).
Sends appointment state change notifications to web clients via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview), thus demonstrating how to use [SignalR](http://signalr.net) in Serverless scenarios.
The actual logic is implemented [here](https://github.com/scale-tone/LetsGoOutDemo/blob/master/backend/LetsGoOutDemo.Functions/LetsGoOutSaga.cs).

## Prerequisites

[Azure Functions Core Tools](https://www.npmjs.com/package/azure-functions-core-tools).

## How to run

* [Get yourself an Azure SignalR Service instance](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core#create-an-azure-signalr-resource).
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
