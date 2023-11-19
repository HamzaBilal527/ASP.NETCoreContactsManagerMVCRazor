using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonGetterServiceWithFewExcelFields : IPersonGetterService
    {
        private readonly PersonGetterService _personGetterService;
        public PersonGetterServiceWithFewExcelFields(PersonGetterService personGetterService)
        {

            _personGetterService = personGetterService;

        }
        public async Task<List<PersonResponse>> GetAllPerson()
        {
            return await _personGetterService.GetAllPerson();
        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            return await _personGetterService.GetFilteredPerson(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            return await _personGetterService.GetPersonByPersonId(personId);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personGetterService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

                excelWorksheet.Cells["A1"].Value = "Persons Name";
                excelWorksheet.Cells["B1"].Value = "Age";
                excelWorksheet.Cells["C1"].Value = "Gender";

                using (ExcelRange excelRange = excelWorksheet.Cells["A1:C1"])
                {
                    excelRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    excelRange.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPerson();

                foreach (PersonResponse person in persons)
                {
                    excelWorksheet.Cells[row, 1].Value = person.PersonName;
                    excelWorksheet.Cells[row, 2].Value = person.Age;
                    excelWorksheet.Cells[row, 3].Value = person.Gender;

                    row++;

                }

                excelWorksheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();


            }

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
