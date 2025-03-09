using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using UKParliament.CodeTest.Infrastructure.DataContexts;

namespace UKParliament.CodeTest.Infrastructure.Repositories;

public class DepartmentRepository(PersonManagerContext context) : IDepartmentRepository
{
    private readonly PersonManagerContext _context = context;

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _context.Departments
            .Include(p => p.People)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<int> GetDepartmentTotalAsync()
    {
        return await _context.Departments.AsNoTracking().CountAsync();
    }

    public async Task<List<Department>> ListDepartmentsAsync()
    {
        return await _context.Departments
            .Include(d => d.People)
            .Select(d => new Department
            {
                Id = d.Id,
                Name = d.Name,
                People = d.People
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
