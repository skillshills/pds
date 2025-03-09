using UKParliament.CodeTest.Domain.Models;

namespace UKParliament.CodeTest.Domain.Repositories;

public interface IPersonRepository
{
    Task<Person> CreatePersonAsync(Person newPerson);

    Task<int> GetPersonTotalAsync();

    Task<List<Person>> GetPersonListAsync();

    Task<Person?> GetPersonByIdAsync(int id);

    Task UpdatePersonAsync(int id, Person updatedPerson);

    Task DeletePersonAsync(int id);
}
