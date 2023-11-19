using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonGetterService
	{
		

		Task<List<PersonResponse>> GetAllPerson();

		Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

		Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);
		Task<MemoryStream> GetPersonsCSV();
		Task<MemoryStream> GetPersonsExcel();

	}
}

