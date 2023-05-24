using EmployeeGrpc;

namespace EmployeeServer;

public class InMemoryCache
{
    public List<EmployeeCreationRequest> Employees { get; }
    public List<int> Ages { get; }

    public InMemoryCache()
    {
        Employees = new List<EmployeeCreationRequest>();
        Ages = new List<int>();
    }
}