using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using UKParliament.CodeTest.Domain.Services;

namespace UKParliament.CodeTest.Application.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{
    private readonly IPersonRepository _personRepository = personRepository;

    public async Task<Person> CreatePersonAsync(Person newPerson)
    {
        return await _personRepository.CreatePersonAsync(newPerson);
    }

    public async Task<Person?> GetPersonByIdAsync(int id)
    {
        return await _personRepository.GetPersonByIdAsync(id);
    }

    public async Task<int> GetPersonTotalAsync()
    {
        return await _personRepository.GetPersonTotalAsync();
    }

    public async Task<List<Person>> GetPersonListAsync()
    {
        return await _personRepository.GetPersonListAsync();
    }

    public async Task UpdatePersonAsync(int id, Person updatedPerson)
    {
        await _personRepository.UpdatePersonAsync(id, updatedPerson);
    }

    public async Task DeletePersonAsync(int id)
    {
        await _personRepository.DeletePersonAsync(id);
    }
}