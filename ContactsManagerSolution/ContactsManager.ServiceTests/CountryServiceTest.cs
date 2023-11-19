using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;
using EntityFrameworkCoreMock;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
	public class CountryServiceTest
	{
		private readonly ICountryService _countryService;

		public CountryServiceTest()
		{
            List<Country> intialCountriesData = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, intialCountriesData);
            _countryService = new CountryService(null);

		}

        #region AddCountry
        //When CountryAddRequest is null
        [Fact]

		public async Task AddCountry_CountryAddRequest_Null()
		{
			CountryAddRequest? countryAddRequest = null;


			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
                await _countryService.AddCountry(countryAddRequest);

            });

		}

        //When CountryNames Null

        [Fact]

        public async Task AddCountry_CountryName_Null()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = null };


            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _countryService.AddCountry(countryAddRequest);

            });

        }

        //When CountryName is duplicate
        [Fact]

        public async Task AddCountry_CountryName_Duplicate()
        {
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "USA"};
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest() { CountryName = "USA" };



            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _countryService.AddCountry(countryAddRequest1);
                await _countryService.AddCountry(countryAddRequest2);


            });

        }

        //When Proper CountryName is provided

        [Fact]

        public async Task AddCountry_CountryName_Proper()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "Japan" };


            
            CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);


            List<CountryResponse> countries_from_GetAllCountries = await _countryService.GetAllCountries();

            Assert.True(countryResponse.CountryId != Guid.Empty);
            Assert.Contains(countryResponse, countries_from_GetAllCountries);



            

        }

        #endregion

        #region GetAllCountries
        [Fact]

        public async Task GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_country_response_list = await _countryService.GetAllCountries();

            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public async Task GetAllCounties_FewCounties()
        {

            List<CountryAddRequest> countries_to_be_added = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){CountryName = "USA"},
                new CountryAddRequest(){CountryName = "UK"}
            };

            List<CountryResponse> country_response_list = new List<CountryResponse>();

            foreach(var added_countries in countries_to_be_added)
            {

               country_response_list.Add(await _countryService.AddCountry(added_countries));

            }

            List<CountryResponse> actual_country_response = await _countryService.GetAllCountries();

            foreach(var expected_countries in country_response_list)
            {
                Assert.Contains(expected_countries, actual_country_response);
            }
        }

        #endregion

        #region GetCountryByCountryId

        [Fact]

        public async Task GetCountryByCountryId_CountryId_Null()
        {
            Guid? countryId = null;

            CountryResponse? countryResponse = await _countryService.GetCountryByCountryId(countryId);

            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task GetCountryByCountryId_ProperCountryId()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse? countryResponse_from_add = await _countryService.AddCountry(countryAddRequest);

            CountryResponse? countryResponse_from_get = await _countryService.GetCountryByCountryId(countryResponse_from_add.CountryId);

            Assert.Equal(countryResponse_from_add, countryResponse_from_get);
        }

        #endregion

    }
}

