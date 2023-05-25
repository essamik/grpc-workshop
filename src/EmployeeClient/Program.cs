using EmployeeClient;
using EmployeeGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

var employees = EmployeesLoader.LoadEmployees();

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new EmployeeStub.EmployeeStubClient(channel);

foreach (var employee in employees)
{
    var reply = await client.AddEmployeeAsync(employee);
    Console.WriteLine("Employee added with id: "+ reply.Id);
}

var medianAge = await client.GetMedianAgeAsync(new Empty());
Console.WriteLine("Median age is: " +  medianAge);