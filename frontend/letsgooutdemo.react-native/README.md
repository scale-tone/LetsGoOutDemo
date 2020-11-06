# LetsGoOutDemo.React-Native

A React Native (more precisely, [Expo](https://docs.expo.io/)) mobile app, written in TypeScript and acting as a UI for [LetsGoOutDemo backend](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend).
Lets you login under some nickName and then send appointment requests to others and accept/reject those appointment requests. Whenever an appointment state changes, the UI gets notified about that via [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview).

## How to run

* Compile and run [any of the backend implementations](https://github.com/scale-tone/LetsGoOutDemo/tree/master/backend) on your devbox or in Azure.
* Create a `config.json` file like that:
```
{
    "backendBaseUri": "<uri-of-your-backend-instance>"
}
```
and put it into this folder (next to `package.json`).
* `npm install`
* `npm run start`

The last command will start **Expo DevTools**, with which you should be able to test/debug the app in browser and/or device emulator. To test the app on your real device you will need to install and use [Expo Development Client](https://expo.io/tools).
