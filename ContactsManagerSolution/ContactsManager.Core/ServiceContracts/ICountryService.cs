using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts;
/// <summary>
/// Represents business logic for manipulating Country Entity
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// Adds a country object to list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country Object to add</param>
    /// <returns>Returns country object as country response after adding it (including new Country id</returns>
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

    Task<List<CountryResponse>> GetAllCountries();

    Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);

    Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
}

