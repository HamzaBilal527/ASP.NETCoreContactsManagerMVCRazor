using CRUDDemo.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDDemo.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;

        public PersonCreateAndEditPostActionFilter(ICountryService countryService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
                    personsController.ViewBag.Countries = countryResponses.Select(temp =>
                    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() }); ;
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personRequest);

                }

                else
                {
                    await next();
                }
            }

            else
            {
                await next();
            }


            _logger.LogInformation("In After Logic of the Action PersonCreateAndEdit Filter");
           
        }
    }
}
