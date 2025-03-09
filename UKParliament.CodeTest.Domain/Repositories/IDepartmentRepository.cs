using UKParliament.CodeTest.Domain.Models;

namespace UKParliament.CodeTest.Domain.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(int id);

    Task<int> GetDepartmentTotalAsync();

    Task<List<Department>> ListDepartmentsAsync();
}
