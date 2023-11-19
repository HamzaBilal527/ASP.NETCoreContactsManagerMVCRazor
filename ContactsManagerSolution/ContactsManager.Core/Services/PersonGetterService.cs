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
    public class PersonGetterService : IPersonGetterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonGetterService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;

        }

        
        

        public virtual async Task<List<PersonResponse>> GetAllPerson()
        {
            _logger.LogInformation("GetAllPersons of PersonService");
            var persons = await _personsRepository.GetAllPersons();
            return persons.Select(temp => temp.ToPersonResponse()).ToList();

        }

        public virtual async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

           Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (person == null)
            {
                return null;
            }

            //return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();

        }

        public virtual async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPersons of PersonService");
            List<Person> person;
            using (SerilogTimings.Operation.Time("Time for Filtered Persons from Database"))
            {


            person = searchBy switch
            {
                nameof(PersonResponse.PersonName) =>
                await _personsRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchString)),

                nameof(PersonResponse.Email) =>
                await _personsRepository.GetFilteredPersons(temp => temp.Email.Contains(searchString)),

                nameof(PersonResponse.DateOfBirth) =>
                await _personsRepository.GetFilteredPersons(temp => temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                nameof(PersonResponse.Gender) =>
                await _personsRepository.GetFilteredPersons(temp => temp.Gender.Contains(searchString)),

                nameof(PersonResponse.CountryId) =>
               await _personsRepository.GetFilteredPersons(temp => temp.Country.CountryName.Contains(searchString)),

                nameof(PersonResponse.Address) =>
                await _personsRepository.GetFilteredPersons(temp => temp.Address.Contains(searchString)),


                _ => await _personsRepository.GetAllPersons()

            };
            }

            _diagnosticContext.Set("Persons", person);
            return person.Select(temp => temp.ToPersonResponse()).ToList();

        }

        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.RecieveNewsLetter));

            csvWriter.NextRecord();

            List<PersonResponse> persons = await GetAllPerson();

            foreach(PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));

                }

                else
                {
                    csvWriter.WriteField("");

                }
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.RecieveNewsLetter);
                csvWriter.NextRecord();
                csvWriter.Flush();

            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using(ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

                excelWorksheet.Cells["A1"].Value = "Persons Name";
                excelWorksheet.Cells["B1"].Value = "Email";
                excelWorksheet.Cells["C1"].Value = "Date of Birth";
                excelWorksheet.Cells["D1"].Value = "Age";
                excelWorksheet.Cells["E1"].Value = "Gender";
                excelWorksheet.Cells["F1"].Value = "Country";
                excelWorksheet.Cells["G1"].Value = "Address";
                excelWorksheet.Cells["H1"].Value = "Recieve News Letter";

                using(ExcelRange excelRange = excelWorksheet.Cells["A1:H1"])
                {
                    excelRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    excelRange.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPerson();

                foreach(PersonResponse person in persons)
                {
                    excelWorksheet.Cells[row, 1].Value = person.PersonName;
                    excelWorksheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                    {
                        excelWorksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");

                    }
                    excelWorksheet.Cells[row, 4].Value = person.Age;
                    excelWorksheet.Cells[row, 5].Value = person.Gender;
                    excelWorksheet.Cells[row, 6].Value = person.Country;
                    excelWorksheet.Cells[row, 7].Value = person.Address;
                    excelWorksheet.Cells[row, 8].Value = person.RecieveNewsLetter;

                    row++;

                }

                excelWorksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();


            }

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}

