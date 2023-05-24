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

    public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
    {
        logger.LogInformation("Message received");
        return Task.FromResult(new MessageResponse()
        {
            Text = "Response to " + request.Text
        });
    }
}