using EmployeeGrpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EmployeeServer.Services;

public class EmployeeService : EmployeeStub.EmployeeStubBase
{
    private readonly ILogger<EmployeeService> logger;
    public EmployeeService(ILogger<EmployeeService> logger)
    {
        this.logger = logger;
    }
    public override Task<EmployeeCreationResponse> AddEmployee(EmployeeCreationRequest request, ServerCallContext context)
    {
        logger.LogInformation("EmployeeCreationRequest received");
        return Task.FromResult(new EmployeeCreationResponse
        {
            Message = request.Name + " successfully created"
        });
    }
}