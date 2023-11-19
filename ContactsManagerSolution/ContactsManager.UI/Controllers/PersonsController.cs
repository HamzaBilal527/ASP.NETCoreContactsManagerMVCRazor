using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CRUDDemo.Filters;
using CRUDDemo.Filters.ActionFilters;
using CRUDDemo.Filters.AuthorizationFilter;
using CRUDDemo.Filters.ExceptionFilters;
using CRUDDemo.Filters.ResourceFilters;
using CRUDDemo.Filters.ResultFilters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfficeOpenXml.Style;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDDemo.Controllers
{
    //[Route("persons")]
    [Route("[controller]")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3)]
    [ResponseHeaderFilterFactory("My-Key-From-Controller", "My-Value-From-Controller", 3)]
    //[TypeFilter(typeof(HandlerExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]


    public class PersonsController : Controller
    {
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonGetterService personGetterService, IPersonAdderService personAdderService, IPersonDeleterService personDeleterService, IPersonSorterService personSorterService, IPersonUpdaterService personUpdaterService, ICountryService countryService, ILogger<PersonsController> logger)
        {
            _personGetterService = personGetterService;
            _personAdderService = personAdderService;
            _personDeleterService = personDeleterService;
            _personSorterService = personSorterService;
            _personUpdaterService = personUpdaterService;
            _countryService = countryService;
            _logger = logger;

        }
        [Route("/")]
        //[Route("index")]
        [Route("[action]")]
        [ServiceFilter(typeof(PersonListActionFilter), Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] {"My-Key-From-Action", "My-Value-From-Action", 1}, Order = 1)]
        [ResponseHeaderFilterFactory("My-Key-From-Action", "My-Value-From-Action", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        public async Task<IActionResult> Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index Action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");
            List<PersonResponse> personResponses1 = await _personGetterService.GetAllPerson();
            List<PersonResponse> personResponses2 =await _personGetterService.GetFilteredPerson(searchBy, searchString);
            List<PersonResponse> personResponses3 = await _personSorterService.GetSortedPerson(personResponses2, sortBy, sortOrder);
            return View(personResponses3);
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpGet]
        [ResponseHeaderFilterFactory("my-key", "my-value", 4)]

        public async Task<IActionResult> Create()
        {
           List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
            ViewBag.Countries = countryResponses.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });

            
            return View();
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //[TypeFilter(typeof(FeatureDisabledResourceFilter))]

        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] {false})]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            //if (!ModelState.IsValid)
            //{
            //    List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
            //    ViewBag.Countries = countryResponses.Select(temp =>
            //    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }); ;
            //    ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
            //    return View(personRequest);

            //}
            PersonResponse personResponse = await _personAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{personId}")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personId)
        {
           PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
            ViewBag.Countries = countryResponses.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });
            return View(personUpdateRequest);
        }

        [Route("[action]/{personId}")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]

        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personRequest.PersonId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            
                PersonResponse personResponse1 = await _personUpdaterService.UpdatePerson(personRequest);
                return RedirectToAction("Index");

            //}

            //else
            //{
            //    List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
            //    ViewBag.Countries = countryResponses.Select(temp =>
            //    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }); ;
            //    ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
            //    return View(personResponse.ToPersonUpdateRequest());

            //}
        }

        [Route("[action]/{personId}")]
        [HttpGet]

        public async Task<IActionResult> Delete(Guid? personId)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [Route("[action]/{personId}")]
        [HttpPost]

        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _personDeleterService.DeletePerson(personUpdateRequest.PersonId);

            return RedirectToAction("Index");
        }

        //Downlaod as PDF

        //[Route("PersonsPDF")]
        //public async Task<IActionResult> PersonsPDF()
        //{
        //    List<PersonResponse> persons = await _personService.GetAllPerson();

        //    return new ViewAsPdf("PersonsPDF", persons, ViewData)
        //    {
        //        PageMargins = new Rotativa.AspNetCore.Options.Margins()
        //        {
        //            Top = 20,
        //            Bottom = 20,
        //            Left = 20,
        //            Right = 20
        //        },

        //        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        //    };
        //}

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}

