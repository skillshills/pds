using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using UKParliament.CodeTest.Infrastructure.DataContexts;

namespace UKParliament.CodeTest.Infrastructure.Repositories;

public class PersonRepository(PersonManagerContext context) : IPersonRepository
{
    private readonly PersonManagerContext _context = context;

    public async Task<Person> CreatePersonAsync(Person newPerson)
    {
        _context.People.Add(newPerson);
        await _context.SaveChangesAsync();
        return newPerson;
    }

    public async Task<Person?> GetPersonByIdAsync(int id)
    {
        return await _context.People
            .Include(p => p.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> GetPersonTotalAsync()
    {
        return await _context.People.AsNoTracking().CountAsync();
    }

    public async Task<List<Person>> GetPersonListAsync()
    {
        return await _context.People
            .Include(d => d.Department)
            .Select(d => new Person
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Department = d.Department,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdatePersonAsync(int id, Person updatedPerson)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return;

        person.FirstName = updatedPerson.FirstName.Trim();
        person.LastName = updatedPerson.LastName.Trim();
        person.DateOfBirth = updatedPerson.DateOfBirth;
        person.DepartmentId = updatedPerson.DepartmentId;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePersonAsync(int id)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return;

        _context.People.Remove(person);
        await _context.SaveChangesAsync();
    }
}