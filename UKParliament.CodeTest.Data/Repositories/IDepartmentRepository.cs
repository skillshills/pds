using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(int id);

    Task<int> GetDepartmentTotalAsync();

    Task<List<Department>> ListDepartmentsAsync();
}
