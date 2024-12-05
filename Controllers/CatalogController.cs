using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using DirectorySite.Models.ViewModel;

namespace DirectorySite.Controllers
{
    [Auth]
    [Route("[controller]")]
    public class CatalogController(ILogger<CatalogController> logger, CatalogService catalogService) : Controller
    {
        #region Fields
        private readonly ILogger<CatalogController> _logger = logger;
        private readonly CatalogService catalogService = catalogService ;
        
        private int locaCountryID = 137;
        private int localStateID = 1;
        private int localMunicipalityId = 1;
        #endregion

        public IActionResult Index()
        {
            return View( );
        }

        [HttpGet]
        [Route("occupations")]
        public async Task<IActionResult> Occupations()
        {
            var data = await catalogService.GetOccupations();
            return View( data );
        }

        [Route("nationalities")]
        public async Task<IActionResult> Nationalities()
        {
            var data = await catalogService.GetNationalities();
            return View( data );
        }

        [Route("marital-status")]
        public async Task<IActionResult> MaritalStatus()
        {
            var data = await catalogService.GetMaritalStatuses();
            return View( data );
        }

        [Route("contact-types")]
        public async Task<IActionResult> ContactTypes()
        {
            var data = await catalogService.GetContactTypes();
            return View( data );
        }
        

        [Route("countries")]
        public async Task<IActionResult> Countries()
        {
            ViewData["Title"] = "Catalogo - Paises";
            var data = await catalogService.GetCountries();
            return View(data);
        }

        [Route("States")]
        public async Task<IActionResult> States(int? countryId )
        {
            if(countryId != null && countryId > 0)
            {
                locaCountryID = countryId.Value;
            }

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];
            this._logger.LogDebug("Total states: {total}", states.Count());
            
            var viewModel = new CatalogStatesViewModel 
            {
                CountryId = locaCountryID,
                Countries = countries.ToList(),
                States = states.ToList()
            };

            ViewData["Title"] = "Catalogo - Estados";
            ViewData["ActivePage"] = "States";
            return View(viewModel);
        }

        [HttpPost("States")]
        public async Task<IActionResult> StoreState([FromForm] int countryId, [FromForm] string name)
        {
            try
            {
                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Errot at attempt to get the countries");
                var country = countries.FirstOrDefault(item => item.Id == countryId) ?? throw new ArgumentException("The countryId was not found on the system", "countryId" );

                // * validate the name
                if(string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("The name must not be null and empty", "name" ); 
                }

               // * register the new data
               var res = await this.catalogService.StoreNewState(countryId, name);
               
                return Ok( new {
                    Message = $"The state '{name}' was created"
                });
            }
            catch(ArgumentException ae)
            {
                return UnprocessableEntity( new {
                    Message = "Error al validar la solicitud",
                    Errors = new Dictionary<string,string>{ { ae.ParamName??"err", ae.Message } }
                });
            }
            catch(Exception e)
            {
                return Conflict( new {
                    Title = "Error al registrar el estado",
                    Message = e.Message,
                });
            }
        }

        [Route("Municipalities")]
        public async Task<IActionResult> Municipalities(int? countryId, int? stateId)
        {
            if(countryId != null && countryId > 0)
            {
                locaCountryID = countryId.Value;
            }

            if(stateId != null && stateId > 0)
            {
                localStateID = stateId.Value;
            }

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];
            var municipalities = (await catalogService.GetMunicipalities(localStateID)) ?? [];
            this._logger.LogDebug("Total Municipios: {total}", states.Count());

            var viewModel = new CatalogStatesViewModel 
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                Countries = countries.ToList(),
                States = states.ToList(),
                Municipalities = municipalities.ToList(),
            };

            ViewData["Title"] = "Catalogo - Municipios";
            ViewData["ActivePage"] = "Municipios";
            return View(viewModel);
        }

        [HttpPost("Municipalities")]
        public async Task<IActionResult> StoreMunicipality([FromForm] int countryId, [FromForm] int stateId, [FromForm] string name)
        {
            try
            {
                _logger.LogInformation("New countryId{id} sateId{sid} name{name}", countryId, stateId, name);

                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Errot at attempt to get the countries");
                var country = countries.FirstOrDefault(item => item.Id == countryId) ?? throw new ArgumentException("The countryId was not found on the system.", "countryId" );
                
                var states = await catalogService.GetStates(countryId) ?? throw new Exception("Errot at attempt to get the states");
                var state = states.FirstOrDefault(item => item.Id == stateId) ?? throw new ArgumentException("The stateId was not found on the system.", "stateId" );
                
                // * validate the name
                if(string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("The name must not be null and empty", "name" ); 
                }

               // * register the new catalog element
               var res = await this.catalogService.StoreNewMunicipality(countryId, stateId, name);

                return Ok( new {
                    Message = $"The state '{name}' was created"
                });
            }
            catch(ArgumentException ae)
            {
                return UnprocessableEntity( new {
                    Message = "Error al validar la solicitud",
                    Errors = new Dictionary<string,string>{ { ae.ParamName??"err", ae.Message } }
                });
            }
            catch(Exception e)
            {
                return Conflict( new {
                    Title = "Error al registrar el estado",
                    Message = e.Message,
                });
            }
        }

        [Route("Colonies")]
        public async Task<IActionResult> Colonies(int? countryId, int? stateId, int? municipalityId )
        {
            if(countryId != null && countryId > 0)
            {
                locaCountryID = countryId.Value;
            }

            if(stateId != null && stateId > 0)
            {
                localStateID = stateId.Value;
            }

            if(municipalityId != null && municipalityId > 0)
            {
                localMunicipalityId = municipalityId.Value;
            }

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];
            var municipalities = (await catalogService.GetMunicipalities(localStateID)) ?? [];
            var colonies = (await catalogService.GetColonies(localMunicipalityId)) ?? [];
            this._logger.LogDebug("Total Municipios: {total}", states.Count());

            var viewModel = new CatalogStatesViewModel 
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                MunicipalityId = localMunicipalityId,
                Countries = countries.ToList(),
                States = states.ToList(),
                Municipalities = municipalities.ToList(),
                Colonies = colonies.ToList()
            };

            ViewData["Title"] = "Catalogo - Colonias";
            ViewData["ActivePage"] = "Colonias";
            return View(viewModel);
        }
        
        [HttpPost("Colonies")]
        public async Task<IActionResult> StoreColonies([FromForm] int countryId, [FromForm] int stateId, [FromForm] int municipalityId, [FromForm] string name, [FromForm] int zipCode)
        {
            try
            {
                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Errot at attempt to get the countries");
                var country = countries.FirstOrDefault(item => item.Id == countryId) ?? throw new ArgumentException("The countryId was not found on the system.", "countryId" );
                
                var states = await catalogService.GetStates(countryId) ?? throw new Exception("Errot at attempt to get the states");
                var state = states.FirstOrDefault(item => item.Id == stateId) ?? throw new ArgumentException("The stateId was not found on the system.", "stateId" );

                var municipalities = await catalogService.GetMunicipalities(stateId) ?? throw new Exception("Errot at attempt to get the municipalities");
                var municipality = municipalities.FirstOrDefault(item => item.Id == municipalityId) ?? throw new ArgumentException("The municipalityId was not found on the system.", "municipalityId" );
                
                // * validate the name
                if(string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("The name must not be null and empty", "name" ); 
                }

               // * register the new catalog element
               var res = await this.catalogService.StoreNewColony(countryId, stateId, municipalityId, name, zipCode);

                return Ok( new {
                    Message = $"The state '{name}' was created"
                });
            }
            catch(ArgumentException ae)
            {
                return UnprocessableEntity( new {
                    Message = "Error al validar la solicitud",
                    Errors = new Dictionary<string,string>{ { ae.ParamName??"err", ae.Message } }
                });
            }
            catch(Exception e)
            {
                return Conflict( new {
                    Title = "Error al registrar el estado",
                    Message = e.Message,
                });
            }
        }


        [HttpPost]
        [Route("occupations")]
        public JsonResult StoreNewOccupation([FromForm] string? name){
            throw new NotImplementedException();
            
            // there is no endpoint on the API for store a new catalog element

            return Json( new {
                Message = "Elemento registrado " + name
            });
        }
    }
}