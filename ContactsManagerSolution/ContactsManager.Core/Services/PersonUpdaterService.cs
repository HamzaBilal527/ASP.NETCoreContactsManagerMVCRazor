using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonUpdaterService : IPersonUpdaterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdaterService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationHelper.ModeValidation(personUpdateRequest);

            Person? person = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (person == null)
            {
                throw new InvalidPersonIdException("Invalid PersonId");
            }

            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.CountryId = personUpdateRequest.CountryId;
            person.Address = personUpdateRequest.Address;
            person.RevieveNewsLetter = personUpdateRequest.RecieveNewsLetter;

            await _personsRepository.UpdatePerson(person );

            //return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();



        }

    }
}

