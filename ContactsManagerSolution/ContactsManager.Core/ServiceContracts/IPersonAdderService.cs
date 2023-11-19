using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonAdderService
	{
		Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
	}
}

