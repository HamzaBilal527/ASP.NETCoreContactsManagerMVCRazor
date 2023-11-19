using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;
public class CountryService : ICountryService
{
    //private readonly List<Country> _countries;
    //private readonly ApplicationDbContext _db;

    private readonly ICountriesRepository _countriesRepository;

    public CountryService(ICountriesRepository countriesRepository)
    {
        //_countries = new List<Country>();
        //if (initialize)
        //{
        //    _countries = new List<Country>() {
        //    new Country() { CountryId = Guid.Parse("da0d12ab-2d86-49e0-8ef1-b04b0331211c"), CountryName = "USA" },
        //    new Country() { CountryId = Guid.Parse("013d2c94-263d-48a0-b8c3-09ea359165df"), CountryName = "UK" },
        //    new Country() { CountryId = Guid.Parse("e557112c-d6e3-4c20-b34b-74377486edca"), CountryName = "Uganda" },
        //    new Country() { CountryId = Guid.Parse("bd1a7769-9b52-4973-9bc2-ab0e09f74d29"), CountryName = "KSA" },
        //    new Country() { CountryId = Guid.Parse("81818d05-bf10-464a-b69e-793fe5e5a186"), CountryName = "AUS" }
        //    };
        //}

        //_db = personsDbContext;
        _countriesRepository = countriesRepository;
    }
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("CountryName already exists");
        }

        Country country = countryAddRequest.ToCountry();

        country.CountryId = Guid.NewGuid();

        await _countriesRepository.AddCountry(country);

        return country.ToCountryResponse();
        
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        List<Country> countries = await _countriesRepository.GetAllCountries();
        return countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
        {
            return null;
        }

        Country? country = await _countriesRepository.GetCountryByCountryId(countryId.Value);

        if (country == null)
        {
            return null;
        }

        return country.ToCountryResponse();
    }

    public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
    {
        MemoryStream memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        int countriesInserted = 0;

        using(ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);

                if (!string.IsNullOrEmpty(cellValue))
                {
                    string? countryName = cellValue;

                    if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
                    {
                        Country country = new Country() { CountryName = countryName };
                        await _countriesRepository.AddCountry(country);

                        countriesInserted++;
                    }
                }
            }
        }

        return countriesInserted;
    }
}

