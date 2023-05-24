using EmployeeGrpc;
using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new EmployeeStub.EmployeeStubClient(channel);

Console.WriteLine("Sending EmployeeCreationRequest to server...");

var reply = await client.SendMessageAsync(
    new MessageRequest() { Text = "Hello world" });

Console.WriteLine("Response from server: " + reply.Text);
