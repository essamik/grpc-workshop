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
    public override Task<EmployeeCreationResponse> AddEmployee(EmployeeCreationRequest employee, ServerCallContext context)
    {
        HandleEmployeeCreationRequest(employee);
        return Task.FromResult(new EmployeeCreationResponse
        {
            Id = employees.Count
        });
    }

    private void HandleEmployeeCreationRequest(EmployeeCreationRequest employee)
    {
        employees.Add(employee);
        if (!string.IsNullOrEmpty(employee.Birthday))
        {
            DateTime birthDate = DateTime.Parse(employee.Birthday);
            DateTime currentDate = DateTime.Now;

            var age = currentDate.Year - birthDate.Year;
            ages.Add(age);
        }

        logger.LogInformation("New Employee added");
    }

    public override Task<MedianAgeResponse> GetMedianAge(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new MedianAgeResponse
        {
            Age = ComputeMedianAge(ages)
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

    public override async Task<MedianAgeResponse> CreateEmployeeAsStreamAndGetMedianAge(IAsyncStreamReader<EmployeeCreationRequest> requestStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            var employee = requestStream.Current;
            HandleEmployeeCreationRequest(employee);
        }

        return new MedianAgeResponse
        {
            Age = ComputeMedianAge(ages)
        };
    }

    public override async Task CreateEmployeeAsStreamAndGetMedianAgeAsStream(IAsyncStreamReader<EmployeeCreationRequest> requestStream, IServerStreamWriter<MedianAgeResponse> responseStream,
        ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            var employee = requestStream.Current;
            HandleEmployeeCreationRequest(employee);

            await responseStream.WriteAsync(new MedianAgeResponse
            {
                Age = ComputeMedianAge(ages)
            });
        }
    }
}