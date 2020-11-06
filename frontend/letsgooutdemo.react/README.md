# LetsGoOutDemo.React

A React+MobX+TypeScript Single-Page App, acting as a UI for [LetsGoOutDemo backend](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend).
Lets you login under some nickName and then send appointment requests to others and accept/reject those appointment requests. Whenever an appointment state changes, the UI gets notified about that via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).

## How to run

* Compile and run [any of the backend implementations](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend) on your devbox.
* Ensure your backend's base URL is set correctly [here](https://github.com/scale-tone/LetsGoOutDemo/blob/master/frontend/letsgooutdemo.react/.env).
* `npm install`
* `npm run start`
