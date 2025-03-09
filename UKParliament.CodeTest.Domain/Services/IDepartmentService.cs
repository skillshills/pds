using UKParliament.CodeTest.Domain.Models;

namespace UKParliament.CodeTest.Domain.Services;

public interface IDepartmentService
{
    Task<Department?> GetDepartmentByIdAsync(int id);

    Task<int> GetDepartmentTotalAsync();

    Task<List<Department>> ListDepartmentsAsync();
}
