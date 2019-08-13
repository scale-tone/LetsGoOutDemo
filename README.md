# LetsGoOutDemo

This demo project demonstrates various ways of implementing the Saga/Workflow design pattern in Azure.

The use case is the same and it is pretty basic: we build a stateful service, that handles organizing appointments with multiple participants. The participants communicate with the service via a client web UI and can submit appointment proposals, accept and/or decline them. The service takes care of maintaining the appointment state and notifies participants about state changes.

* [**LetsGoOutDemo.Functions**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend/letsgooutdemo.functions#letsgooutdemofunctions) - an Azure Durable Functions backend (C#), that implements the Appointments Saga. Appointment state change notifications are sent via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).
* [**LetsGoOutDemo.React**](https://github.com/scale-tone/LetsGoOutDemo/tree/master/frontend/letsgooutdemo.react#letsgooutdemoreact) - a React+MobX+TypeScript simple web UI, that communicates with the backend and with [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).
