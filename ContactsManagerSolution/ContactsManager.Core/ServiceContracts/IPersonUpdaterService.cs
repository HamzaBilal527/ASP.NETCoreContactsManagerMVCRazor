using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonUpdaterService
	{
		Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

	}
}

