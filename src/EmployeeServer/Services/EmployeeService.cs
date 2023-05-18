using EmployeeGrpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EmployeeServer.Services;

public class EmployeeService : EmployeeStub.EmployeeStubBase
{
    private readonly ILogger<EmployeeService> _logger;
    public EmployeeService(ILogger<EmployeeService> logger)
    {
        _logger = logger;
    }
    public override Task<EmployeeCreationResponse> AddEmployee(EmployeeCreationRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EmployeeCreationResponse
        {
            Message = "Hello " + request.Name
        });
    }
}