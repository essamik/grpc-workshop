using EmployeeGrpc;
using Newtonsoft.Json;

namespace EmployeeClient;

public class EmployeesLoader
{
    public static List<EmployeeCreationRequest> LoadEmployees()
    {
        var jsonString = File.ReadAllText("employees.json");
        return JsonConvert.DeserializeObject<List<EmployeeCreationRequest>>(jsonString,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            })!;
    }
}