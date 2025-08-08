using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

// let's create a client that can be used to create senders and receivers 
ServiceBusClient client;

// processor to read and process messages from the queue 
ServiceBusProcessor processor;

// set the transport type to AmqpWebSockets for ServiceBusClient to use
var clientOptions = new ServiceBusClientOptions()
{
    TransportType =  ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient(
    "<NAMESPACE-NAME>.servicebus.windows.net",
    new DefaultAzureCredential(),
    clientOptions
);

// create a process to proces the messages 
processor = client.CreateProcessor("<QUEUE-NAME>", new ServiceBusProcessorOptions());


try
{
    // add handler to process messages 
    processor.ProcessMessageAsync += MessageHandler;

    // add handler to process any errors 
    processor.ProcessErrorAsync += ErrorHandler;

    // start processing 
    await processor.StartProcessingAsync();
    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    // stop processing
    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}

finally
{
    // calling DisposeAsync on client types to ensure network
    // resources and other unmanaged resources are cleaned up
    await processor.DisposeAsync();
    await client.DisposeAsync();
}


// handle received messages 
async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received message: {body}");

    // complete the message. message is deleted from the queue.
    await args.CompleteMessageAsync(args.Message);

}

// handle any errors while receiving messages 
Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}