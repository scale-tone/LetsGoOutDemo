# LetsGoOutDemo

This demo project demonstrates various ways of implementing the Saga/Workflow design pattern in Azure.

The use case is the same and it is pretty basic: we build a stateful service, that handles organizing appointments with multiple participants. The participants communicate with the service via a client web UI and can submit appointment proposals, accept and/or decline them. The service takes care of maintaining the appointment state and notifies participants about state changes.