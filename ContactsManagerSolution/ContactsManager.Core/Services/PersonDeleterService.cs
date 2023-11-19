﻿using System;
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
    public class PersonDeleterService : IPersonDeleterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleterService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {

            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException("personId cant be empty or null");
            }

            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (person == null)
            {
                return false;
            }

            await _personsRepository.DeletePersonByPersonId(personId.Value);

            return true;
        }

    }
}

