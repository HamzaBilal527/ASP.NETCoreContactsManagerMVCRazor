using System;
using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	public class PersonUpdateRequest
	{
        public Guid PersonId { get; set; }
        [Required(ErrorMessage = "Person Name cant be Null or Empty")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email cant be null or empty")]
        [EmailAddress(ErrorMessage = "Must provide Valid Email")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetter { get; set; }


        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                RevieveNewsLetter = RecieveNewsLetter
            };
        }
    }
}

