using EmployeeGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EmployeeServer.Services;

public class EmployeeService : EmployeeStub.EmployeeStubBase
{
    private readonly ILogger<EmployeeService> logger;
    private readonly InMemoryCache cache;

    public EmployeeService(ILogger<EmployeeService> logger, InMemoryCache cache)
    {
        this.logger = logger;
        this.cache = cache;
    }
    public override Task<EmployeeCreationResponse> AddEmployee(EmployeeCreationRequest employee, ServerCallContext context)
    {
        HandleEmployeeCreationRequest(employee);
        return Task.FromResult(new EmployeeCreationResponse
        {
            Id = cache.Employees.Count
        });
    }

    private void HandleEmployeeCreationRequest(EmployeeCreationRequest employee)
    {
        cache.Employees.Add(employee);
        if (!string.IsNullOrEmpty(employee.Birthday))
        {
            DateTime birthDate = DateTime.Parse(employee.Birthday);
            DateTime currentDate = DateTime.Now;

            var age = currentDate.Year - birthDate.Year;
            cache.Ages.Add(age);
        }

        logger.LogInformation("New Employee added");
    }

    public override Task<MedianAgeResponse> GetMedianAge(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new MedianAgeResponse
        {
            Age = ComputeMedianAge(cache.Ages)
        });
    }

    private static int ComputeMedianAge(List<int> array)
    {
        array.Sort();

        int length = array.Count;
        int middleIndex = length / 2;

        int median;

        if (length % 2 == 0)
        {
            median = (array[middleIndex - 1] + array[middleIndex]) / 2;
        }
        else
        {
            median = array[middleIndex];
        }

        return median;
    }
}