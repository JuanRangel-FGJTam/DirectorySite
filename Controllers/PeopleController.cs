using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using DirectorySite.Models.ViewModel;

namespace DirectorySite.Controllers
{

    [Auth]
    [Route("[controller]")]
    public class PeopleController(ILogger<PeopleController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, PeopleSearchService _peopleSearchService, PeopleService _peopleService, PeopleSessionService _peopleSessionService, PeopleProcedureService _peopleProcedureService, CatalogService _catalogService) : Controller
    {
        private readonly ILogger<PeopleController> _logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IConfiguration configuration = configuration;
        private readonly PeopleSearchService peopleSearchService = _peopleSearchService;
        private readonly PeopleService peopleService = _peopleService;
        private readonly PeopleSessionService peopleSessionService = _peopleSessionService;
        private readonly PeopleProcedureService peopleProcedureService = _peopleProcedureService;
        private readonly CatalogService catalogService = _catalogService;

        public async Task<IActionResult> Index([FromQuery] string? search)
        {
            var viewModel = new PeopleIndexViewModel
            {
                Search = search    
            };
            
            if(string.IsNullOrEmpty(search))
            {
                return View(viewModel);
            }

            try
            {
                viewModel.People = await this.peopleSearchService.SearchPerson(search!) ?? [];
                return View(viewModel);
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at search the person");
                ViewBag.ErrorMessage = "Error al realizar la busqueda, " + err.Message;
                return View(viewModel);
            }
        }


        [HttpGet]
        [Route("{personID}")]
        public async Task<IActionResult> Person([FromRoute] string personID, [FromQuery] string? searchText){
            PersonResponse? personResponse = null;
            try
            {
                personResponse = await this.peopleService.GetPersonById(personID);
            }
            catch(UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "People");
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the person '{personId}'", personID);
            }

            if(!string.IsNullOrEmpty(searchText))
            {
                ViewBag.SearchText = searchText;
            }
            
            return View(personResponse);
        }

        [HttpGet]
        [Route("{personID}/edit")]
        public async Task<IActionResult> EditPerson([FromRoute] string personID)
        {
            PersonResponse? personResponse = null;
            try
            {
                personResponse = await this.peopleService.GetPersonById(personID);
            }
            catch(UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "People");
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the person '{personId}'", personID);
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    Message = "Error al obtene los datos del usuario."
                });
            }
            
            // *  verify if the person is found 
            if(personResponse == null)
            {
                ViewBag.NotFoundMessage = "La persona no se encuntra registrada en el sistema";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            // * prepare request models
            ViewBag.UpdatePersonGeneralsRequest = await InitializedUpdatePersonGeneralsRequest(personResponse);
            ViewBag.UpdatePersonContactRequest = await InitializedUpdatePersonContactRequest(personResponse);

            // * return the view
            ViewData["Title"] = $"Editando Persona {personResponse.FullName}";
            return View("PersonEdit", personResponse);

        }

        [HttpPatch]
        [Route("{personID}/generals")]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateGeneralData([FromRoute] string personID, [FromForm] UpdatePersonGeneralsRequest request)
        {

            // TODO: Validate the request

            // * attempt to get the person
            PersonResponse? personResponse = null;
            try
            {
                personResponse = await this.peopleService.GetPersonById(personID);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the person '{personId}'", personID);
                return Conflict( new {
                    Message = "Error al obtene los datos del usuario."
                });
            }

            // * update the person
            try
            {
                var response = await peopleService.UpdatePerson(personID, request);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Conflict();
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new {
                    ae.Message
                });
            }
            catch (InvalidDataException)
            {
                return Conflict();
            }
            
        }

        [HttpPatch]
        [Route("{personID}/contact")]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateContactInformation([FromRoute] string personID, [FromForm] UpdatePersonContactRequest request)
        {
            // * attempt to get the person
            PersonResponse? personResponse = null;
            try
            {
                personResponse = await this.peopleService.GetPersonById(personID);
            }
            catch(UnauthorizedAccessException)
            {
                this._logger.LogInformation("Redirect");
                return Unauthorized();
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the data of the person '{personId}'", personID);
                return Conflict( new {
                    Message = "Error al obtene los datos del usuario."
                });
            }

            // * update the contact information of the person
            try
            {
                var response = await peopleService.UpdatePersonEmail(personID, request.Email!); 
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new {
                    ae.Message
                });
            }
            catch (InvalidDataException)
            {
                return Conflict();
            }
        }

        #region PartialViews
        [HttpGet]
        [Route("{personID}/sessions")]
        public async Task<IActionResult> GetSessionPartialView([FromRoute] string personID){
            SessionsResponse? sessionsData = null;
            try
            {
                sessionsData = await this.peopleSessionService.GetSessionsOfPerson(personID, take:5, skip:0);
                return PartialView("~/Views/People/Partials/PersonSessions.cshtml", sessionsData);
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the sessions data of the person '{personId}'", personID);
                ViewData["ErrorTitle"] = "Error al obtener las sesiones del usuario";
                ViewData["ErrorMessage"] = "Hubo un error al obtener las sesiones del usuario, intente de nuevo o comuníquese con un administrador.";
                return PartialView("~/Views/Shared/ErrorAlert.cshtml", new ErrorViewModel());
            }
        }

        [HttpGet]
        [Route("{personID}/procedures")]
        public async Task<IActionResult> GetProceduresPartialView([FromRoute] string personID){
            IEnumerable<ProcedureResponse> procedureResponses = [];
            try
            {
                procedureResponses = await this.peopleProcedureService.GetProceduresOfPerson(personID, take:5, skip:0);
                return PartialView("~/Views/People/Partials/PersonProcedures.cshtml", procedureResponses);
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Fail at get the procedures data of the person '{personId}'", personID);
                ViewData["ErrorTitle"] = "Error al obtener los procedimientos del usuario";
                ViewData["ErrorMessage"] = "Hubo un error al obtener las procedimientos del usuario, intente de nuevo o comuníquese con un administrador.";
                return PartialView("~/Views/Shared/ErrorAlert.cshtml", new ErrorViewModel());
            }
        }
        #endregion

        #region private functions
        private async Task<UpdatePersonGeneralsRequest> InitializedUpdatePersonGeneralsRequest(PersonResponse personResponse){

            // * get the catalogs
            IEnumerable<Gender>? genders = [];
            IEnumerable<MaritalStatus>? maritalStatuses = [];
            IEnumerable<Nationality>? nationalities = [];
            IEnumerable<Occupation>? occupations = [];
            var catalogsTask = new List<Task>
            {
                Task.Run(async () =>
                {
                    genders = await catalogService.GetGenders() ?? [];
                }),
                Task.Run(async () =>
                {
                    maritalStatuses = await catalogService.GetMaritalStatuses() ?? [];
                }),
                Task.Run(async () =>
                {
                    nationalities = await catalogService.GetNationalities() ?? [];
                }),
                Task.Run(async () =>
                {
                    occupations = await catalogService.GetOccupations() ?? [];
                })
            };
            await Task.WhenAll(catalogsTask);

            // * prepare request models
            var updatePersonGeneralsRequest = new UpdatePersonGeneralsRequest {
                Name = personResponse.Name,
                FirstName = personResponse.FirstName,
                LastName = personResponse.LastName,
                Curp = personResponse.Curp,
                Rfc = personResponse.Rfc,
                Birthdate = personResponse.Birthdate,
                GenderId = personResponse.GenderId,
                MaritalStatusId = personResponse.MaritalStatusId,
                NationalityId = personResponse.NationalityId,
                OccupationId = personResponse.OccupationId,
                Genders = genders.Select( g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(g.Name, g.Id.ToString())),
                MaritalStatuses = maritalStatuses.Select( g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(g.Name, g.Id.ToString())),
                Nationalities = nationalities.Select( g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(g.Name, g.Id.ToString())),
                Occupations = occupations.Select( g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(g.Name, g.Id.ToString())),
            };

            return updatePersonGeneralsRequest;
        }

        private async Task<UpdatePersonContactRequest> InitializedUpdatePersonContactRequest(PersonResponse personResponse){

            // * get the catalogs
            IEnumerable<ContactType>? contactTypes = await catalogService.GetContactTypes() ?? [];

            // * prepare request models
            var updatePersonGeneralsRequest = new UpdatePersonContactRequest {
                Email = personResponse.Email,
                ContactTypes = contactTypes.Select( g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(g.Name, g.Id.ToString())),
            };

            return updatePersonGeneralsRequest;
        }

        #endregion

    }
}