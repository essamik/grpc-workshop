using EmployeeGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EmployeeServer.Services;

public class EmployeeService : EmployeeStub.EmployeeStubBase
{
    private static List<EmployeeCreationRequest> employees;
    private static List<int> ages;
    private readonly ILogger<EmployeeService> logger;
    public EmployeeService(ILogger<EmployeeService> logger)
    {
        this.logger = logger;
        if (employees == null)
        {
            employees = new List<EmployeeCreationRequest>();
        }
        if (ages == null)
        {
            ages = new List<int>();
        }
    }
    public override Task<EmployeeCreationResponse> AddEmployee(EmployeeCreationRequest request, ServerCallContext context)
    {
        employees.Add(request);
        if (!string.IsNullOrEmpty(request.Birthday))
        {
            DateTime birthDate = DateTime.Parse(request.Birthday);
            DateTime currentDate = DateTime.Now;

            var age = currentDate.Year - birthDate.Year;
            ages.Add(age);
        }
        logger.LogInformation("New Employee added");
        return Task.FromResult(new EmployeeCreationResponse
        {
            Id = employees.Count
        });
    }

    public override Task<MedianAgeResponse> GetMedianAge(Empty request, ServerCallContext context)
    {
        ages.Sort();

        int length = ages.Count;
        int middleIndex = length / 2;

        int median;

        if (length % 2 == 0)
        {
            median = (ages[middleIndex - 1] + ages[middleIndex]) / 2;
        }
        else
        {
            median = ages[middleIndex];
        }

        return Task.FromResult(new MedianAgeResponse
        {
            Age = median
        });
    }
}