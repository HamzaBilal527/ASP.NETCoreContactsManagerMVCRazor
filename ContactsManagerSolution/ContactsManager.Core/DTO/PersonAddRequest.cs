using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
	public class PersonAddRequest
	{
		[Required(ErrorMessage = "Person Name cant be Null or Empty")]
		public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email cant be null or empty")]
		[EmailAddress(ErrorMessage = "Must provide Valid Email")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }
		[Required(ErrorMessage = "Please provide a gender")]
		public GenderOptions? Gender { get; set; }
		[Required(ErrorMessage = "Please Select a country")]
		public Guid? CountryId { get; set; }
		public string? Address { get; set; }
		public bool RecieveNewsLetter { get; set; }


		public Person ToPerson()
		{
			return new Person() { PersonName = PersonName, Email = Email,
				DateOfBirth = DateOfBirth, Gender = Gender.ToString(), CountryId = CountryId,
				Address = Address, RevieveNewsLetter = RecieveNewsLetter
			};
		}
	}
}

