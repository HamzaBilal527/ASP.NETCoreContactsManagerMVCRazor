using System;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using AutoFixture.Kernel;
using Serilog.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
	public class PersonServiceTest
	{
		private readonly IPersonGetterService _personGetterService;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly IPersonUpdaterService _personUpdaterService;

        //private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        private readonly IPersonsRepository _personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;

		public PersonServiceTest(ITestOutputHelper testOutputHelper)
		{
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;
            var diganosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<ILogger<PersonGetterService>>();

            //List<Country> intialCountriesData = new List<Country>() { };
            //List<Person> intialPersonsData = new List<Person>() { };


            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            //ApplicationDbContext dbContext = dbContextMock.Object;

            //dbContextMock.CreateDbSetMock(temp => temp.Countries, intialCountriesData);
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, intialPersonsData);


            //_countryService = new CountryService(null);
            _personGetterService = new PersonGetterService(_personsRepository, loggerMock.Object, diganosticContextMock.Object);
            _personAdderService = new PersonAdderService(_personsRepository, loggerMock.Object, diganosticContextMock.Object);
            _personDeleterService = new PersonDeleterService(_personsRepository, loggerMock.Object, diganosticContextMock.Object);
            _personSorterService = new PersonSorterService(_personsRepository, loggerMock.Object, diganosticContextMock.Object);
            _personUpdaterService = new PersonUpdaterService(_personsRepository, loggerMock.Object, diganosticContextMock.Object);
            _testOutputHelper = testOutputHelper;
		}


        #region AddPerson
        [Fact]
		public async Task AddPerson_PersonAddRequest_IsNull_ArgumentNullException()
		{
			PersonAddRequest? personAddRequest = null;

			//await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			//{
			//	await _personService.AddPerson(personAddRequest);
			//});

            Func<Task> action = async () =>
            {
                await _personAdderService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

		}

        [Fact]
        public async Task AddPerson_PersonName_IsNull_ArgumentException()
        {
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            Person person = personAddRequest.ToPerson();

            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            //         PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //	PersonName = null
            //};

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () =>
            {
                await _personAdderService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some@example.com").Create();
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Hamza", Email = "hamzabilal21@gmail.com", Address = "Eden Value Homes",
            //    CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("1995-10-27"), Gender = GenderOptions.Male,
            //    RecieveNewsLetter = true
            //};
            Person person = personAddRequest.ToPerson();
            PersonResponse personResponse_expected = person.ToPersonResponse();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            PersonResponse personResponse_from_add = await _personAdderService.AddPerson(personAddRequest);
            personResponse_expected.PersonId = personResponse_from_add.PersonId;

            //List<PersonResponse> personResponses_list = await _personService.GetAllPerson();

            //Assert.True(personResponse.PersonId != Guid.Empty);

            //Assert.Contains(personResponse, personResponses_list);

            personResponse_from_add.PersonId.Should().NotBe(Guid.Empty);

            personResponse_from_add.Should().Be(personResponse_expected);
        }

        #endregion

        #region GetPersonByPersonId

        [Fact]
        public async Task GetPersonByPersonId_NullPersonId_ToBeNull()
        {
            Guid? personId = null;

            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personId);

            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonId_ValidPersonId_ToBeSuccessfull()
        {
            //CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>().Create();
            //CountryAddRequest countryAddRequest = new CountryAddRequest()
            //{
            //    CountryName = "Canada"
            //};

            //CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Hamza", Email = "hamzabilal21@gmail.com", Address = "C217, EVH", CountryId = countryResponse.CountryId,
            //    DateOfBirth = DateTime.Parse("1997-10-27"), Gender = GenderOptions.Male, RecieveNewsLetter = false
            //};

            Person person = _fixture.Build<Person>().With(temp => temp.Email, "some@exampel.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            PersonResponse personResponse_expected = person.ToPersonResponse();

            //PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);
            PersonResponse? personResponse_fron_get = await _personGetterService.GetPersonByPersonId(person.PersonId);

            //Assert.Equal(personResponse, personResponse1);

            personResponse_fron_get.Should().Be(personResponse_expected);
        }
        #endregion

        #region GetAllPerson

        [Fact]
        public async Task GetAllPerson_EmptyList()
        {

            List<Person> persons = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);
            //Assert.Empty(personResponse_list);
            List<PersonResponse> personResponse_list = await _personGetterService.GetAllPerson();

            personResponse_list.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPerson_WithFewPerson_ToBeSuccessful()
        {
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            //CountryAddRequest countryAddRequest1 = _fixture.Build<CountryAddRequest>().Create();
            //CountryAddRequest countryAddRequest2 = _fixture.Build<CountryAddRequest>().Create();

            //CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
            //CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some1@exampel.com").With(temp => temp.PersonName, "Rahman")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();


            //PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some2@exampel.com").With(temp => temp.PersonName, "Mary")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();

            //PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some3@exampel.com").With(temp => temp.PersonName, "scott")
            //    .With(temp => temp.CountryId, countryResponse2.CountryId).Create();

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Muhammad",
            //    Address = "Muhammad Address",
            //    Email = "Muhammad@email.com",
            //    CountryId = countryResponse1.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-10-9"),
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = true
            //};


            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Hamza",
            //    Address = "Hamza Address",
            //    Email = "Hamza@email.com",
            //    CountryId = countryResponse1.CountryId,
            //    DateOfBirth = DateTime.Parse("2001-10-9"),
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = false
            //};

            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Bilal",
            //    Address = "Bilal Address",
            //    Email = "Bilal@email.com",
            //    CountryId = countryResponse2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-10-9"),
            //    Gender = GenderOptions.Female,
            //    RecieveNewsLetter = true
            //};

            List<Person> persons = new List<Person>()
            {
             _fixture.Build<Person>().With(temp => temp.Email, "some1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some2@example.com")
             .With(temp => temp.Country, null as Country)
            .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some3@example.com")
             .With(temp => temp.Country, null as Country)
            .Create()

            };


            List<PersonResponse> personResponse_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();
        //List<PersonAddRequest> personAddRequests_list = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3};

            //List<PersonResponse> personResponses_list_add = new List<PersonResponse>();

            //foreach(var personAddRequest in personAddRequests_list)
            //{
            //    PersonResponse personResponse =await _personService.AddPerson(personAddRequest);
            //    personResponses_list_add.Add(personResponse);
            //}

            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse personResponse_from_add in personResponse_list_expected)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

           List<PersonResponse> personResponses_list_get = await _personGetterService.GetAllPerson();

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personResponses_list_get)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //foreach (var personResponse_from_add in personResponses_list_add)
            //{
            //    Assert.Contains(personResponse_from_add, personResponses_list_get);
            //}

            personResponses_list_get.Should().BeEquivalentTo(personResponse_list_expected);
            

        }
        #endregion

        #region GetFilteredPerson
        [Fact]
        public async Task GetFilteredPerson_EmptySearchText_ToBeSuccessful()
        {
            //CountryAddRequest countryAddRequest1 = _fixture.Build<CountryAddRequest>().Create();
            //CountryAddRequest countryAddRequest2 = _fixture.Build<CountryAddRequest>().Create();

            //CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
            //CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some1@exampel.com").With(temp => temp.PersonName, "Rahman")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();


            //PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some2@exampel.com").With(temp => temp.PersonName, "Mary")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();

            //PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some3@exampel.com").With(temp => temp.PersonName, "scott")
            //    .With(temp => temp.CountryId, countryResponse2.CountryId).Create();

            //List<PersonAddRequest> personAddRequests_list = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };
            List<Person> persons = new List<Person>()
            {
             _fixture.Build<Person>().With(temp => temp.Email, "some1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some2@example.com")
             .With(temp => temp.Country, null as Country)
            .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some3@example.com")
             .With(temp => temp.Country, null as Country)
            .Create()

            };


            List<PersonResponse> personResponse_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> personResponses_list_add = new List<PersonResponse>();

            //foreach (var personAddRequest in personAddRequests_list)
            //{
            //    PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            //    personResponses_list_add.Add(personResponse);
            //}

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personResponse_list_expected)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);


            List<PersonResponse> personResponses_list_search = await _personGetterService.GetFilteredPerson(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personResponses_list_search)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //foreach (var personResponse_from_add in personResponses_list_add)
            //{
            //    Assert.Contains(personResponse_from_add, personResponses_list_search);
            //}

            personResponses_list_search.Should().BeEquivalentTo(personResponse_list_expected);

        }

        [Fact]
        public async Task GetFilteredPerson_SearchByPerosnName_ToBeSuccessful()
        {
            //CountryAddRequest countryAddRequest1 = _fixture.Build<CountryAddRequest>().Create();
            //CountryAddRequest countryAddRequest2 = _fixture.Build<CountryAddRequest>().Create();

            //CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
            //CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some1@exampel.com").With(temp => temp.PersonName, "Rahman")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();


            //PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some2@exampel.com").With(temp => temp.PersonName, "Mary")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();

            //PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some3@exampel.com").With(temp => temp.PersonName, "scott")
            //    .With(temp => temp.CountryId, countryResponse2.CountryId).Create();

            //List<PersonAddRequest> personAddRequests_list = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            //List<PersonResponse> personResponses_list_add = new List<PersonResponse>();

            //foreach (var personAddRequest in personAddRequests_list)
            //{
            //    PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            //    personResponses_list_add.Add(personResponse);
            //}

            //_testOutputHelper.WriteLine("Expected:");
            //foreach (PersonResponse personResponse_from_add in personResponses_list_add)
            //{
            //    _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            //}

            //List<PersonResponse> personResponses_list_search = await _personService.GetFilteredPerson(nameof(Person.PersonName), "ma");

            //_testOutputHelper.WriteLine("Actual:");
            //foreach (PersonResponse personResponse_from_get in personResponses_list_search)
            //{
            //    _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            //}

            ////foreach (var personResponse_from_add in personResponses_list_add)
            ////{
            ////    if (personResponse_from_add.PersonName != null)
            ////    {
            ////        if (personResponse_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            ////        {
            ////              Assert.Contains(personResponse_from_add, personResponses_list_search);

            ////        }
            ////    }

            ////}

            //personResponses_list_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
            List<Person> persons = new List<Person>()
            {
             _fixture.Build<Person>().With(temp => temp.Email, "some1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some2@example.com")
             .With(temp => temp.Country, null as Country)
            .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some3@example.com")
             .With(temp => temp.Country, null as Country)
            .Create()

            };

            List<PersonResponse> personResponse_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> personResponses_list_add = new List<PersonResponse>();

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personResponse_list_expected)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);


            List<PersonResponse> personResponses_list_search = await _personGetterService.GetFilteredPerson(nameof(Person.PersonName), "sa");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personResponses_list_search)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            personResponses_list_search.Should().BeEquivalentTo(personResponse_list_expected);
        }
        #endregion

        #region GetSortedPerson
        [Fact]
        public async Task GetSortedPerson_ToBeSuccessful()
        {
            //CountryAddRequest countryAddRequest1 = _fixture.Build<CountryAddRequest>().Create();
            //CountryAddRequest countryAddRequest2 = _fixture.Build<CountryAddRequest>().Create();

            //CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
            //CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some1@exampel.com").With(temp => temp.PersonName, "Rahman")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();


            //PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some2@exampel.com").With(temp => temp.PersonName, "Mary")
            //    .With(temp => temp.CountryId, countryResponse1.CountryId).Create();

            //PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "some3@exampel.com").With(temp => temp.PersonName, "scott")
            //    .With(temp => temp.CountryId, countryResponse2.CountryId).Create();
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            //CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
            //CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Muhammad",
            //    Address = "Muhammad Address",
            //    Email = "Muhammad@email.com",
            //    CountryId = countryResponse1.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-10-9"),
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = true
            //};


            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Hamza",
            //    Address = "Hamza Address",
            //    Email = "Hamza@email.com",
            //    CountryId = countryResponse1.CountryId,
            //    DateOfBirth = DateTime.Parse("2001-10-9"),
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = false
            //};

            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Bilal",
            //    Address = "Bilal Address",
            //    Email = "Bilal@email.com",
            //    CountryId = countryResponse2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-10-9"),
            //    Gender = GenderOptions.Female,
            //    RecieveNewsLetter = true
            //};

            //List<PersonAddRequest> personAddRequests_list = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            //List<PersonResponse> personResponses_list_add = new List<PersonResponse>();

            //foreach (var personAddRequest in personAddRequests_list)
            //{
            //    PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            //    personResponses_list_add.Add(personResponse);
            //}
            List<Person> persons = new List<Person>()
            {
             _fixture.Build<Person>().With(temp => temp.Email, "some1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some2@example.com")
             .With(temp => temp.Country, null as Country)
            .Create(),
            _fixture.Build<Person>().With(temp => temp.Email, "some3@example.com")
             .With(temp => temp.Country, null as Country)
            .Create()

            };

            List<PersonResponse> personResponse_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponse_from_add in personResponse_list_expected)
            {
                _testOutputHelper.WriteLine(personResponse_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personGetterService.GetAllPerson();
            List<PersonResponse> personResponses_list_sort = await _personSorterService.GetSortedPerson(allPersons, nameof( Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponse_from_get in personResponses_list_sort)
            {
                _testOutputHelper.WriteLine(personResponse_from_get.ToString());
            }

            //personResponses_list_add = personResponses_list_add.OrderByDescending(temp => temp.PersonName).ToList();

            //for (int i = 0; i < personResponses_list_add.Count; i++)
            //{
            //    Assert.Equal(personResponses_list_add[i], personResponses_list_sort[i]);
            //}

            //personResponses_list_sort.Should().BeEquivalentTo(personResponses_list_add);

            personResponses_list_sort.Should().BeInDescendingOrder(temp => temp.PersonName);

        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_Null_PersonUpdateRequest_ArgumentNullException()
        {
            PersonUpdateRequest? personUpdateRequest = null;


            //await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            //{
            //   await _personService.UpdatePerson(personUpdateRequest);

            //});

            Func<Task> action = async () =>
            {
                await _personUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ArgumentException()
        {
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.UpdatePerson(personUpdateRequest);

            //});

            Func<Task> action = async () =>
            {
                await _personUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_Null_PersonName_ArgumentException()
        {
            //CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>().Create();
            

            //CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
           

            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "some1@exampe.com")
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Gender, "Male")
                .With(temp => temp.Country, null as Country).Create();

            PersonResponse personResponse_add = person.ToPersonResponse();
            PersonUpdateRequest? personUpdateRequest = personResponse_add.ToPersonUpdateRequest();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //   await _personService.UpdatePerson(personUpdateRequest);

            //});

            Func<Task> action = async () =>
            {
                await _personUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_FullPersonDetails_ToBeSuccessful()
        {
            //CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>().Create();


            //CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);


            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "some1@exampe.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse personResponse_expected = person.ToPersonResponse();
            PersonUpdateRequest? personUpdateRequest = personResponse_expected.ToPersonUpdateRequest();


            _personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);


            PersonResponse personResponse_update = await _personUpdaterService.UpdatePerson(personUpdateRequest);
            //PersonResponse? personResponse_get = await _personService.GetPersonByPersonId(personResponse_update.PersonId);



            //Assert.Equal(personResponse_get, personResponse_update);

            personResponse_update.Should().Be(personResponse_expected);
        }
        #endregion

        #region DeletePerson

        [Fact]
        public async Task DeletePerson_Null_PersonId()
        {
            Guid? personId = null;


            //await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            //{
            //    await _personService.DeletePerson(personId);

            //});

            Func<Task> action = async () =>
            {
                await _personDeleterService.DeletePerson(personId);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeletePerson_Invalid_PersondId()
        {
            Guid? personId = Guid.NewGuid();

            bool isDeleted = await _personDeleterService.DeletePerson(personId);

            //Assert.False(isDeleted);

            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_Valid_PersonId()
        {
            //CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>().Create();


            //CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);


            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "some1@exampe.com")
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse personResponse_add = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);
            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            bool isDeleted =  await _personDeleterService.DeletePerson(person.PersonId);

            //Assert.True(isDeleted);

            isDeleted.Should().BeTrue();
        }
        #endregion
    }
}

