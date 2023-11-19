using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonDeleterService
	{
		Task<bool> DeletePerson(Guid? personId);
	}
}

