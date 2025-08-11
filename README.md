# Azure Service Bus Queue Learning Project

This project demonstrates how to work with Azure Service Bus Queues using C# and .NET. It contains two console applications: a **Sender** that publishes messages to a queue and a **Receiver** that consumes messages from the queue.

## Overview

Azure Service Bus is a fully managed enterprise message broker with message queues and publish-subscribe topics. This project focuses on **queues**, which provide First-In-First-Out (FIFO) message delivery to one or more competing consumers.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- An Azure subscription
- Azure Service Bus namespace and queue
- Visual Studio Code or Visual Studio (recommended)

## Project Structure

```
ServiceBusQueue.sln
├── Sender/                 # Console app that sends messages
│   ├── Program.cs         # Main sender logic
│   └── Sender.csproj      # Project file with dependencies
└── Receiver/              # Console app that receives messages
    ├── Program.cs         # Main receiver logic
    └── Receiver.csproj    # Project file with dependencies
```

## Setup

### 1. Clone or Download the Project
```bash
git clone <your-repository-url>
cd service-bus-queue
```

### 2. Create Azure Service Bus Resources
1. Go to [Azure Portal](https://portal.azure.com)
2. Create a new **Service Bus Namespace**
3. Create a **Queue** within the namespace
4. Get the connection string from "Shared access policies"

### 3. Install Dependencies
```bash
dotnet restore
```

## Configuration

### Passwordless Authentication (Recommended)
This project uses **Azure Identity** for passwordless authentication.

1. **Login to Azure CLI** (for local development):
```bash
az login
```

2. **Set Environment Variables**:
```bash
export AZURE_SERVICE_BUS_NAMESPACE="<your-namespace>.servicebus.windows.net"
export AZURE_SERVICE_BUS_QUEUE_NAME="demo-queue"
```

3. **Assign Service Bus Permissions**:
```bash
az role assignment create \
  --role "Azure Service Bus Data Owner" \
  --assignee <your-email> \
  --scope /subscriptions/<subscription-id>/resourceGroups/<resource-group>/providers/Microsoft.ServiceBus/namespaces/<namespace-name>
```

### Alternative: Connection String
For testing purposes, you can use a connection string:
```bash
export AZURE_SERVICE_BUS_CONNECTION_STRING="Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<key>"
```

## Running the Applications

### Build the Solution
```bash
dotnet build
```

### Run the Receiver (Start this first)
```bash
cd Receiver
dotnet run
```

### Run the Sender (In a new terminal)
```bash
cd Sender
dotnet run
```

## Learning Objectives

By working with this project, you'll learn:
- Basic message queuing and producer-consumer patterns
- Azure Service Bus SDK usage
- Asynchronous programming with async/await
- Message handling (sending, receiving, processing)
- Azure Identity for passwordless authentication

## Key Concepts

### Service Bus Queue Features
- **FIFO Delivery**: Messages are delivered in order
- **At-Least-Once Delivery**: Messages are guaranteed to be delivered
- **Competing Consumers**: Multiple receivers can process messages
- **Dead Letter Queue**: Failed messages are moved to a dead letter queue

### Important SDK Classes
- `ServiceBusClient`: Main client for connecting to Service Bus
- `ServiceBusSender`: Sends messages to a queue
- `ServiceBusReceiver`: Receives messages from a queue
- `ServiceBusMessage`: Represents a message
- `DefaultAzureCredential`: Handles authentication

## Next Steps

### Enhancements to Try
- Add message properties and custom headers
- Implement batch processing
- Add dead letter handling
- Implement monitoring and logging
- Add error handling and retry policies

## Resources

- [Azure Service Bus Documentation](https://docs.microsoft.com/azure/service-bus-messaging/)
- [.NET SDK Reference](https://docs.microsoft.com/dotnet/api/azure.messaging.servicebus)
- [Azure Identity Documentation](https://docs.microsoft.com/dotnet/api/overview/azure/identity-readme)
