using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonGetterServiceChildClass : PersonGetterService
    {
        public PersonGetterServiceChildClass(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext) : base(personsRepository, logger, diagnosticContext)
        {
            
        }
        public async override Task<MemoryStream> GetPersonsExcel()
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

                if (persons.Count == 0)
                {
                    throw new InvalidOperationException("No Persons Data");
                }

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
