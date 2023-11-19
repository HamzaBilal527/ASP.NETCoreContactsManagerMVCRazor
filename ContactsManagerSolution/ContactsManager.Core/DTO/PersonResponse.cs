using System;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
	public class PersonResponse
	{
		public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetter { get; set; }
        public double? Age { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            PersonResponse personResponse = (PersonResponse)obj;

            return PersonId == personResponse.PersonId && PersonName == personResponse.PersonName &&
                     Email == personResponse.Email && DateOfBirth == personResponse.DateOfBirth &&
                     Gender == personResponse.Gender && CountryId == personResponse.CountryId &&
                     Address == personResponse.Address && RecieveNewsLetter == personResponse.RecieveNewsLetter
                     && Age == personResponse.Age && Country == personResponse.Country;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"PersonId: {PersonId}, PersonNamme: {PersonName}, Email: {Email}, DateOfBirth: {DateOfBirth?.ToString("dd MM yyyy")}, " +
                $"Gender: {Gender}, Address: {Address}, CountryId: {CountryId}, Country: {Country}, RecieveNewsLetter: {RecieveNewsLetter}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Address = Address,
                CountryId = CountryId,
                RecieveNewsLetter = RecieveNewsLetter
            };
        }

    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                RecieveNewsLetter = person.RevieveNewsLetter,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName
            };

        }

        
    }
}

