using EmployeeClient;
using EmployeeGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

var employees = EmployeesLoader.LoadEmployees();

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new EmployeeStub.EmployeeStubClient(channel);

await UnaryCall(employees, client);

// await ClientStreamingCall(client, employees);

// await BiDirectionalStreamingCall(client, employees);

async Task UnaryCall(List<EmployeeCreationRequest> employeeCreationRequests, EmployeeStub.EmployeeStubClient employeeStubClient)
{
    foreach (var employee in employeeCreationRequests)
    {
        var reply = await employeeStubClient.AddEmployeeAsync(employee);
        Console.WriteLine("Employee added with id: "+ reply.Id);
    }

    var medianAge = await employeeStubClient.GetMedianAgeAsync(new Empty());
    Console.WriteLine("Median age is: " +  medianAge);
}

async Task ClientStreamingCall(EmployeeStub.EmployeeStubClient client1, List<EmployeeCreationRequest> list)
{
    using var call = client1.CreateEmployeeAsStreamAndGetMedianAge();
    Console.WriteLine("Starting Stream");

    foreach (var employee in list)
    {
        await call.RequestStream.WriteAsync(employee);
    }

    await call.RequestStream.CompleteAsync();

    var response = await call;
    Console.WriteLine($"Median age is: {response.Age}");
}

async Task BiDirectionalStreamingCall(EmployeeStub.EmployeeStubClient employeeStubClient1, List<EmployeeCreationRequest> employees1)
{
    using var call = employeeStubClient1.CreateEmployeeAsStreamAndGetMedianAgeAsStream();

    Console.WriteLine("Starting Stream");
    foreach (var employee in employees1)
    {
        await call.RequestStream.WriteAsync(employee);
    }

    await call.RequestStream.CompleteAsync();
    while (await call.ResponseStream.MoveNext(CancellationToken.None))
    {
        var medianAgeResponse = call.ResponseStream.Current;
        Console.WriteLine($"Median age is: {medianAgeResponse.Age}");
    }
}
