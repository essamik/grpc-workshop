using EmployeeGrpc;
using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new EmployeeStub.EmployeeStubClient(channel);
var reply = await client.AddEmployeeAsync(
    new EmployeeCreationRequest() { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
