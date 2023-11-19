using System;
using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts
{
	public interface IPersonsRepository
	{
		Task<Person> AddPerson(Person person);

		Task<List<Person>> GetAllPersons();

		Task<Person?> GetPersonByPersonId(Guid personId);

		Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

		Task<bool> DeletePersonByPersonId(Guid personId);

		Task<Person> UpdatePerson(Person person);
	}
}

