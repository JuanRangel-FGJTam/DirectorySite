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