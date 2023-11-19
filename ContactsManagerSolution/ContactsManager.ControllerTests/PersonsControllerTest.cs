using AutoFixture;
using Castle.Core.Logging;
using CRUDDemo.Controllers;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonsController> _logger;

        private readonly Mock<IPersonGetterService> _personGetterServiceMock;
        private readonly Mock<IPersonAdderService> _personAdderServiceMock;
        private readonly Mock<IPersonDeleterService> _personDeleterServiceMock;
        private readonly Mock<IPersonSorterService> _personSorterServiceMock;
        private readonly Mock<IPersonUpdaterService> _personUpdaterServiceMock;

        private readonly Mock<ICountryService> _countryServiceMock;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        private readonly Fixture _fixture;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();

            _personGetterServiceMock = new Mock<IPersonGetterService>();
            _personAdderServiceMock = new Mock<IPersonAdderService>();
            _personDeleterServiceMock = new Mock<IPersonDeleterService>();
            _personSorterServiceMock = new Mock<IPersonSorterService>();
            _personUpdaterServiceMock = new Mock<IPersonUpdaterService>();

            _countryServiceMock = new Mock<ICountryService>();

            _personGetterService = _personGetterServiceMock.Object;
            _personAdderService = _personAdderServiceMock.Object;
            _personDeleterService = _personDeleterServiceMock.Object;
            _personSorterService = _personSorterServiceMock.Object;
            _personUpdaterService = _personUpdaterServiceMock.Object;
            _countryService =  _countryServiceMock.Object;

            _loggerMock = new Mock<ILogger<PersonsController>>();
            _logger = _loggerMock.Object;


        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            List<PersonResponse> person_response_list = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personGetterService, _personAdderService, _personDeleterService, _personSorterService, _personUpdaterService, _countryService, _logger);

            _personGetterServiceMock.Setup(temp => temp.GetFilteredPerson(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(person_response_list);

            _personSorterServiceMock.Setup(temp => temp.GetSortedPerson(It.IsAny<List<PersonResponse>>() ,It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(person_response_list);

           IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

           ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();

            viewResult.ViewData.Model.Should().Be(person_response_list);


        }
        #endregion

        #region Create

        //[Fact]
        //public async Task Create_ModelErrors_ToReturnCreateView()
        //{
        //    PersonAddRequest person_add_requuest = _fixture.Create<PersonAddRequest>();

        //    PersonResponse person_response = _fixture.Create<PersonResponse>();

        //    List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();


        //    PersonsController personsController = new PersonsController(_personService, _countryService, _logger);

        //    _countryServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

        //    _personServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

        //    personsController.ModelState.AddModelError("PersonName", "Person Name cant be blank");

        //    IActionResult result = await personsController.Create(person_add_requuest);

        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);

        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();

        //    viewResult.ViewData.Model.Should().Be(person_add_requuest);
        //}

        [Fact]
        public async Task Create_NoModelErrors_ToReturnRedirectToIndex()
        {
            PersonAddRequest person_add_requuest = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();


            PersonsController personsController = new PersonsController(_personGetterService, _personAdderService, _personDeleterService, _personSorterService, _personUpdaterService, _countryService, _logger);

            _countryServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

            _personAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);


            IActionResult result = await personsController.Create(person_add_requuest);

            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion

    }
}
