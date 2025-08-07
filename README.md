# Azure Service Bus Queue Learning Project

This project demonstrates how to work with Azure Service Bus Queues using C# and .NET. It contains two console applications: a **Sender** that publishes messages to a queue and a **Receiver** that consumes messages from the queue.

## Table of Contents
- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Setup](#setup)
- [Configuration](#configuration)
- [Running the Applications](#running-the-applications)
- [Learning Objectives](#learning-objectives)
- [Key Concepts](#key-concepts)
- [Next Steps](#next-steps)
- [Resources](#resources)

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

#### Using Azure Portal:
1. Go to [Azure Portal](https://portal.azure.com)
2. Create a new **Service Bus Namespace**
3. Create a **Queue** within the namespace
4. Get the connection string from "Shared access policies"

#### Using Azure CLI:
```bash
# Create resource group
az group create --name rg-servicebus-demo --location eastus

# Create Service Bus namespace
az servicebus namespace create --name <unique-namespace-name> --resource-group rg-servicebus-demo --sku Standard

# Create a queue
az servicebus queue create --name demo-queue --namespace-name <namespace-name> --resource-group rg-servicebus-demo

# Get connection string
az servicebus namespace authorization-rule keys list --resource-group rg-servicebus-demo --namespace-name <namespace-name> --name RootManageSharedAccessKey
```

### 3. Install Dependencies
```bash
# From the project root
dotnet restore
```

## Configuration

### Passwordless Authentication Setup (Recommended)
This project uses **Azure Identity** for passwordless authentication, which is Microsoft's recommended approach for security and best practices.

#### 1. Configure Azure Service Bus Permissions
Assign the appropriate role to your Azure account or managed identity:

```bash
# Get your Azure account object ID
az ad signed-in-user show --query id -o tsv

# Assign Service Bus Data Owner role (for both sending and receiving)
az role assignment create \
  --role "Azure Service Bus Data Owner" \
  --assignee <your-object-id> \
  --scope /subscriptions/<subscription-id>/resourceGroups/<resource-group>/providers/Microsoft.ServiceBus/namespaces/<namespace-name>
```

**Available Service Bus Roles:**
- `Azure Service Bus Data Owner`: Full access (send, receive, manage)
- `Azure Service Bus Data Sender`: Send messages only
- `Azure Service Bus Data Receiver`: Receive messages only

#### 2. Set Environment Variables
Set the Service Bus namespace and queue name:

```bash
export AZURE_SERVICE_BUS_NAMESPACE="<your-namespace>.servicebus.windows.net"
export AZURE_SERVICE_BUS_QUEUE_NAME="demo-queue"
```

#### 3. Authentication Methods

**For Local Development:**
```bash
# Login to Azure CLI (this sets up default credentials)
az login
```

**For Production (using Managed Identity):**
- Enable System-Assigned or User-Assigned Managed Identity on your Azure resource
- Assign the appropriate Service Bus role to the managed identity
- No additional configuration needed in code

#### 4. Configuration Files (Optional)
Create `appsettings.json` in both Sender and Receiver folders:
```json
{
  "ServiceBus": {
    "Namespace": "<your-namespace>.servicebus.windows.net",
    "QueueName": "demo-queue"
  }
}
```

### Alternative: Connection String (Not Recommended for Production)
If you need to use connection strings for testing:

```bash
# Environment variable
export AZURE_SERVICE_BUS_CONNECTION_STRING="Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<key>"

# Or user secrets
dotnet user-secrets set "ServiceBusConnectionString" "your-connection-string-here"
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

1. **Basic Message Queuing**: Understanding producer-consumer patterns
2. **Azure Service Bus SDK**: Using the `Azure.Messaging.ServiceBus` library
3. **Asynchronous Programming**: Working with async/await patterns
4. **Message Handling**: Sending, receiving, and processing messages
5. **Error Handling**: Dealing with connection issues and message failures
6. **Authentication**: Using Azure Identity for passwordless authentication

## Key Concepts

### Service Bus Queue Features
- **FIFO Delivery**: Messages are delivered in order
- **At-Least-Once Delivery**: Messages are guaranteed to be delivered
- **Competing Consumers**: Multiple receivers can process messages
- **Dead Letter Queue**: Failed messages are moved to a dead letter queue
- **Message TTL**: Messages can have time-to-live settings

### Message Lifecycle
1. **Send**: Producer sends message to queue
2. **Receive**: Consumer receives and locks message
3. **Process**: Consumer processes the message
4. **Complete**: Consumer marks message as processed (removes from queue)
5. **Abandon**: Consumer releases lock (message becomes available again)

### Important SDK Classes
- `ServiceBusClient`: Main client for connecting to Service Bus (supports Azure Identity)
- `ServiceBusSender`: Sends messages to a queue or topic
- `ServiceBusReceiver`: Receives messages from a queue or subscription
- `ServiceBusMessage`: Represents a message
- `ServiceBusReceivedMessage`: Represents a received message
- `DefaultAzureCredential`: Automatically handles authentication flow

### Passwordless Authentication Benefits
- **Enhanced Security**: No secrets to manage or rotate
- **Simplified Management**: Automatic credential resolution
- **Production Ready**: Works seamlessly with Managed Identity
- **Development Friendly**: Uses your Azure CLI login for local development
- **Zero Trust**: Follows Microsoft's Zero Trust security model

## Next Steps

### Enhancements to Try
1. **Add message properties**: Include custom headers and properties
2. **Implement batch processing**: Send and receive multiple messages at once
3. **Add dead letter handling**: Process messages that fail repeatedly
4. **Implement sessions**: Use session-aware message processing
5. **Add monitoring**: Implement logging and metrics
6. **Error handling**: Add retry policies and circuit breakers

### Advanced Features to Explore
- **Topics and Subscriptions**: Publish-subscribe messaging
- **Auto-forwarding**: Chain queues together
- **Scheduled messages**: Send messages for future delivery
- **Duplicate detection**: Automatic duplicate message detection
- **Partitioning**: Scale out with partitioned entities

## Resources

### Documentation
- [Azure Service Bus Documentation](https://docs.microsoft.com/azure/service-bus-messaging/)
- [.NET SDK Reference](https://docs.microsoft.com/dotnet/api/azure.messaging.servicebus)
- [Service Bus Pricing](https://azure.microsoft.com/pricing/details/service-bus/)

### Tutorials
- [Service Bus Quickstart with Azure Identity](https://docs.microsoft.com/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues)
- [Azure Identity Documentation](https://docs.microsoft.com/dotnet/api/overview/azure/identity-readme)
- [Passwordless Authentication Guide](https://docs.microsoft.com/azure/developer/intro/passwordless-overview)
- [Best Practices](https://docs.microsoft.com/azure/service-bus-messaging/service-bus-performance-improvements)

### Related Azure Services
- **Azure Event Hubs**: High-throughput event streaming
- **Azure Event Grid**: Event-driven architectures
- **Azure Storage Queues**: Simple message queuing
