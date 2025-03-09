using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Services;

public interface IDepartmentService
{
    Task<Department?> GetDepartmentByIdAsync(int id);

    Task<int> GetDepartmentTotalAsync();

    Task<List<Department>> ListDepartmentsAsync();
}
