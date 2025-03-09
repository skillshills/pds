using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using UKParliament.CodeTest.Domain.Services;

namespace UKParliament.CodeTest.Application.Services;

public class DepartmentService(IDepartmentRepository departmentRepository) : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _departmentRepository.GetDepartmentByIdAsync(id);
    }

    public async Task<int> GetDepartmentTotalAsync()
    {
        return await _departmentRepository.GetDepartmentTotalAsync();
    }

    public async Task<List<Department>> ListDepartmentsAsync()
    {
        return await _departmentRepository.ListDepartmentsAsync();
    }
}
