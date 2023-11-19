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
    public class PersonAdderService : IPersonAdderService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {

            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;

        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationHelper.ModeValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();

            person.PersonId = Guid.NewGuid();

            await _personsRepository.AddPerson(person);
           
            return person.ToPersonResponse();
            

        }

       
    }
}

