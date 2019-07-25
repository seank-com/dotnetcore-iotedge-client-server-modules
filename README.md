# dotnetcore-iotedge-client-server-modules
A simple set of .Net Core IoTEdge modules demonstrating client server interactions

## Build
Open solution in Visual Studio 2019 or later and press <kdb>F5</kbd>

## Purpose

The IoTEdge currently doesn't support routing messages between modules running on different IoTEdge devices. For solutions that need to have multiple IoTEdge devices coordinate and synthesize telemetry, we need a module listening on an open port that other modules can call. We assume the devices that need communicate with each other are located on the same local area network and that the IP addresses are static such that the server's IP can be sent to the client in the module manifest.

**NOT CURRENTLY WORKING**

*There are a few open issues with Docker for Windows containers that are keeping this project from functioning correctly. Hopefully, I'll have time to return to this sample as Docker for Windows containers matures.*