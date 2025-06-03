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
        
        private int locaCountryID = 138;
        private int localStateID = 28;
        private int localMunicipalityId = 41;
        #endregion

        public IActionResult Index()
        {
            return View( );
        }

        [HttpGet]
        [Route("occupations")]
        public async Task<IActionResult> Occupations(string? searchText)
        {
            var data = await catalogService.GetOccupations() ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                data = data!.Where(item => item.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            var viewModel = new CatalogViewModel
            {
                Data = data,
                SearchText = searchText
            };
            
            return View(viewModel);
        }

        [Route("nationalities")]
        public async Task<IActionResult> Nationalities(string? searchText)
        {
            var data = await catalogService.GetNationalities() ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                data = data.Where(item => item.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)).ToArray();
            }
            
            var viewModel = new CatalogViewModel
            {
                Data = data,
                SearchText = searchText
            };
            return View(viewModel);
        }

        [Route("marital-status")]
        public async Task<IActionResult> MaritalStatus(string? searchText)
        {
            var data = await catalogService.GetMaritalStatuses() ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                data = data.Where(item => item.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
            }

            var viewModel = new CatalogViewModel
            {
                Data = data,
                SearchText = searchText
                
            };
            return View(viewModel);
        }

        [Route("contact-types")]
        public async Task<IActionResult> ContactTypes(string? searchText)
        {
            var data = await catalogService.GetContactTypes() ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                data = data.Where(item => item.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
            }
            var viewModel = new CatalogViewModel
            {
                Data = data,
                SearchText = searchText
                
            };
            return View(viewModel);
        }
        

        [Route("countries")]
        public async Task<IActionResult> Countries(string? searchText)
        {
            ViewData["Title"] = "Catalogo - Paises";
            var countries = await catalogService.GetCountries();

            if (!string.IsNullOrEmpty(searchText))
            {
                countries = countries!.Where(item => item.Name.Contains(searchText!, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            var viewModel = new CatalogStatesViewModel()
            {
                Countries = countries ?? [],
                SearchText = searchText
            };
            
            return View(viewModel);
        }

        [Route("States")]
        public async Task<IActionResult> States(int? countryId, string? searchText )
        {
            if(countryId != null && countryId > 0)
            {
                locaCountryID = countryId.Value;
            }

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                states = await catalogService.GetStates(countryId:0, search: searchText) ?? [];
            }
            this._logger.LogDebug("Total states: {total}", states.Count());
            
            var viewModel = new CatalogStatesViewModel
            {
                CountryId = locaCountryID,
                Countries = countries.ToList(),
                States = states.ToList(),
                SearchText = searchText
            };

            ViewData["Title"] = "Catalogo - Estados";
            ViewData["ActivePage"] = "States";
            return View(viewModel);
        }

        [Route("states/new")]
        public async Task<IActionResult> CreateState(int? countryId)
        {
            if(countryId != null && countryId > 0)
            {
                locaCountryID = countryId.Value;
            }

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];
            
            var viewModel = new NewCatalogStatesViewModel
            {
                CountryId = locaCountryID,
                Countries = countries.ToList(),
                States = states.ToList()
            };
            ViewData["Title"] = "Catalogo - Registrar nuevo Estado";
            ViewData["ActivePage"] = "Nuevo Estado";
            return View("StateNew", viewModel);
        }

        [HttpPost("states/new")]
        public async Task<IActionResult> StoreState(NewCatalogStatesViewModel request)
        {
            try
            {
                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Error al obtener el catalog de paises");
                var country = countries.FirstOrDefault(item => item.Id == request.CountryId!) ?? throw new ArgumentException("El pais selecciondo no se encuentra en el sistema", "countryId");

                // * validate the name
                if (string.IsNullOrEmpty(request.StateName))
                {
                    throw new ArgumentException("The name must not be null and empty", "name");
                }

                // * register the new data
                var res = await this.catalogService.StoreNewState(request.CountryId!.Value, request.StateName);

                this._logger.LogInformation("The state '{stateName}' was created", request.StateName);
                return RedirectToAction("States");
            }
            catch (ArgumentException ae)
            {
                var errors = new Dictionary<string, string> { { ae.ParamName ?? "err", ae.Message } };
                ViewBag.ErrorMessage = string.Join("\n", errors.Values);

                // * load again the catalogs
                request.Countries = await catalogService.GetCountries() ?? [];

                // * return the view
                return View("StateNew", request);
            }
            catch (Exception e)
            {
                return Conflict(new
                {
                    Title = "Error al registrar el estado",
                    Message = e.Message,
                });
            }
        }

        [Route("Municipalities")]
        public async Task<IActionResult> Municipalities(int? countryId, int? stateId, string? searchText)
        {
            locaCountryID = countryId ?? 138;
            localStateID = stateId ?? 28;
            
            // * get the coutry and states catalogs
            var countries = await catalogService.GetCountries() ?? [];
            var states = await catalogService.GetStates(locaCountryID) ?? [];

            // * the stateId is not in the country catalog, update them
            if (!states.Select(item => item.Id).Contains(localStateID))
            {
                localStateID = states.FirstOrDefault()?.Id ?? -1;
            }

            // * fetch the municipalities
            var municipalities = (await catalogService.GetMunicipalities(localStateID)) ?? [];
            
            // * if search param is passed, ignore the filters and serach the municipalities
            if (!string.IsNullOrEmpty(searchText))
            {
                municipalities = await catalogService.GetMunicipalities(stateId: 0, search: searchText) ?? [];
            }
            this._logger.LogDebug("LocaCountryID: {country}, LocalStateID: {stateId}, Total Municipios: {total} ", locaCountryID, localStateID, municipalities.Count());

            // * prepare the view
            var viewModel = new CatalogStatesViewModel
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                Countries = countries.ToList(),
                States = states.ToList(),
                Municipalities = municipalities.ToList(),
                SearchText = searchText
            };
            ViewData["Title"] = "Catalogo - Municipios";
            ViewData["ActivePage"] = "Municipios";
            return View(viewModel);
        }

        [Route("municipalities/new")]
        public async Task<IActionResult> CreateMunicipality(int? countryId, int? stateId)
        {
            locaCountryID = countryId ?? 0;
            localStateID = stateId ?? 0;

            var countries = (await catalogService.GetCountries()) ?? [];
            var states = (await catalogService.GetStates(locaCountryID)) ?? [];

            // * override the stateId if is not in the catalog
            if (!states.Select(item => item.Id).Contains(localStateID))
            {
                localStateID = states.FirstOrDefault()?.Id ?? -1;
            }

            // * return the view
            var viewModel = new NewCatalogStatesViewModel
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                Countries = countries.ToList(),
                States = states.ToList()
            };
            ViewData["Title"] = "Catalogo - Registrar nuevo Municipio";
            ViewData["ActivePage"] = "Nuevo Municipio";
            return View("MunicipalitiesNew", viewModel);
        }

        [HttpPost("municipalities/new")]
        public async Task<IActionResult> StoreMunicipality(NewCatalogStatesViewModel request)
        {
            try
            {
                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Erro al obtener el catalogo de paises.");
                var country = countries.FirstOrDefault(item => item.Id == request.CountryId) ?? throw new ArgumentException("El pais seleccionado no se encontro en el sistema.", "CountryId" );
                
                var states = await catalogService.GetStates(request.CountryId!.Value) ?? throw new Exception("No hay estados disponibles para el pais seleccionado.");
                var state = states.FirstOrDefault(item => item.Id == request.StateId!.Value) ?? throw new ArgumentException("El estado seleccionado no se encuentra en el sistema.", "StateId" );
                
                // * validate the name
                if(string.IsNullOrEmpty(request.MunicipalityName))
                {
                    throw new ArgumentException("El nombre del nuevo municipio es requerido.", "name" );
                }

                // * register the new catalog element
                var res = await this.catalogService.StoreNewMunicipality(request.CountryId!.Value, request.StateId!.Value, request.MunicipalityName);

                return RedirectToAction("Municipalities");
            }
            catch(ArgumentException ae)
            {
                var errors = new Dictionary<string, string> { { ae.ParamName ?? "err", ae.Message } };
                ViewBag.ErrorMessage = string.Join("\n", errors.Values);

                // * load again the catalogs
                request.Countries = await catalogService.GetCountries() ?? [];
                request.States = await catalogService.GetStates(request.CountryId!.Value) ?? [];

                // * return the view
                return View("MunicipalitiesNew", request);
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
        public async Task<IActionResult> Colonies(int? countryId, int? stateId, int? municipalityId, string? searchText)
        {
            locaCountryID = countryId ?? 138;
            localStateID = stateId ?? 28;
            localMunicipalityId = municipalityId ?? 41;

            // * get the coutry and states catalogs
            var countries = await catalogService.GetCountries() ?? [];

            var states = await catalogService.GetStates(locaCountryID) ?? [];
            // * override the localStateID if is not ont he catalog
            if (!states.Select(item => item.Id).Contains(localStateID))
            {
                localStateID = states.FirstOrDefault()?.Id ?? -1;
            }

            var municipalities = await catalogService.GetMunicipalities(localStateID) ?? [];
            // * override the municipalityId if is not ont he catalog
            if (!municipalities.Select(item => item.Id).Contains(localMunicipalityId))
            {
                localMunicipalityId = municipalities.FirstOrDefault()?.Id ?? -1;
            }

            var colonies = await catalogService.GetColonies(localMunicipalityId) ?? [];

            // * if search param is passed, ignore the filters and serach the municipalities
            if (!string.IsNullOrEmpty(searchText))
            {
                colonies = await catalogService.GetColonies(municipalityId: 0, search: searchText) ?? [];
            }
            this._logger.LogDebug("Total Municipios: {total}", states.Count());

            // * prepare the view
            var viewModel = new CatalogStatesViewModel
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                MunicipalityId = localMunicipalityId,
                Countries = countries.ToList(),
                States = states.ToList(),
                Municipalities = municipalities.ToList(),
                Colonies = colonies.ToList(),
                SearchText = searchText
            };
            ViewData["Title"] = "Catalogo - Colonias";
            ViewData["ActivePage"] = "Colonias";
            return View(viewModel);
        }
        
        [Route("colonies/new")]
        public async Task<IActionResult> CreateColony(int? countryId, int? stateId, int? municipalityId)
        {
            locaCountryID = countryId ?? 138;
            localStateID = stateId ?? 28;
            localMunicipalityId = municipalityId ?? 41;

            var countries = await catalogService.GetCountries() ?? [];

            var states = await catalogService.GetStates(locaCountryID) ?? [];
            // * override the stateId if is not in the catalog
            if (!states.Select(item => item.Id).Contains(localStateID))
            {
                localStateID = states.FirstOrDefault()?.Id ?? -1;
            }

            var municipalities = await catalogService.GetMunicipalities(localStateID) ?? [];
            if (!municipalities.Select(item => item.Id).Contains(localMunicipalityId))
            {
                localMunicipalityId = municipalities.FirstOrDefault()?.Id ?? -1;
            }

            // * return the view
            var viewModel = new NewCatalogStatesViewModel
            {
                CountryId = locaCountryID,
                StateId = localStateID,
                MunicipalityId = localMunicipalityId,
                Countries = countries.ToList(),
                States = states.ToList(),
                Municipalities = municipalities
            };
            ViewData["Title"] = "Catalogo - Registrar nueva Colonia";
            ViewData["ActivePage"] = "Nueva Colonia";
            return View("ColoniesNew", viewModel);
        }

        [HttpPost("colonies/new")]
        public async Task<IActionResult> StoreColonies(NewCatalogStatesViewModel request)
        {
            try
            {
                // * get the country related
                var countries = await catalogService.GetCountries() ?? throw new Exception("Erro al obtener el catalogo de paises.");
                var country = countries.FirstOrDefault(item => item.Id == request.CountryId) ?? throw new ArgumentException("El pais seleccionado no se encontro en el sistema.", "CountryId" );
                
                var states = await catalogService.GetStates(request.CountryId!.Value) ?? throw new Exception("No hay estados disponibles para el pais seleccionado.");
                var state = states.FirstOrDefault(item => item.Id == request.StateId!.Value) ?? throw new ArgumentException("El estado seleccionado no se encuentra en el sistema.", "StateId" );

                var municipalities = await catalogService.GetMunicipalities(request.StateId!.Value) ?? throw new Exception("No hay municipios disponibles para el estado seleccionado");
                var municipality = municipalities.FirstOrDefault(item => item.Id == request.MunicipalityId!.Value) ?? throw new ArgumentException("El municipio seleccionado no se encuentra en el sistema.", "municipalityId" );
                
                // * validate the name
                if(string.IsNullOrEmpty(request.ColonyName))
                {
                    throw new ArgumentException("The nombre del la colonia es requerido", "name" );
                }

                // * register the new catalog element
                var res = await this.catalogService.StoreNewColony(country.Id!.Value, state.Id!.Value, municipality.Id!.Value, request.ColonyName, int.Parse(request.ZipCode ?? "0"));

                return RedirectToAction("Colonies");
            }
            catch(ArgumentException ae)
            {
                var errors = new Dictionary<string, string> { { ae.ParamName ?? "err", ae.Message } };
                ViewBag.ErrorMessage = string.Join("\n", errors.Values);

                // * load again the catalogs
                request.Countries = await catalogService.GetCountries() ?? [];
                request.States = await catalogService.GetStates(request.CountryId!.Value) ?? [];
                var _stateID = request.States.FirstOrDefault()?.Id ?? -1;
                request.Municipalities = await catalogService.GetMunicipalities(_stateID) ?? [];

                // * return the view
                return View("ColoniesNew", request);
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
            // * there is no endpoint on the API for store a new catalog element
            // return Json( new {
            //     Message = "Elemento registrado " + name
            // });
        }
    
        [HttpGet("SearchZipcode")]
        public async Task<IActionResult> SearchZipcode(int? zipcode)
        {
            ViewData["Title"] = "Catalogo - Codigo Postal";
            ViewData["ActivePage"] = "Codigo Postal";

            if(zipcode == null)
            {
                return View();
            }

            // * search th code data
            try
            {
                var searchResult = await this.catalogService.SearchZipcode(zipcode.Value);
                return View(searchResult);
            }
            catch(KeyNotFoundException knfe)
            {
                ViewBag.WarningMessage = knfe.Message;
                return View();
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex, "Fail at get the data");
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    
        #region Document Types
        [Route("document-types")]
        public async Task<IActionResult> DocumentTypes(string? searchText)
        {
            var data = await catalogService.GetDocumentTypes() ?? [];
            if (!string.IsNullOrEmpty(searchText))
            {
                data = data.Where(item => item.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
            }
            var viewModel = new CatalogViewModel
            {
                Data = data,
                SearchText = searchText
            };
            return View(viewModel);
        }
        
        [HttpDelete("document-types/{documentTypeId}")]
        public async Task<IActionResult> DeleteDocumentType([FromRoute] int documentTypeId)
        {
            try
            {
                await this.catalogService.DeleteDocumentType(documentTypeId);
                return Ok();
            }
            catch(Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("document-types/new")]
        public IActionResult CreateDocumentType()
        {
            return View("DocumentTypeNew");
        }

        [HttpPost("document-types")]
        public async Task<IActionResult> StoreDocumentType(DocumentType model)
        {
            try
            {
                await this.catalogService.StoreDocumentType(model.Name);
            }
            catch(Exception err)
            {
                ViewBag.ErrorMessage = err.Message;
                return View("DocumentTypeNew", model);
            }
            return RedirectToAction("DocumentTypes");
        }

        [HttpGet("document-types/{documentTypeId}")]
        public async Task<IActionResult> EditDocumentType([FromRoute] int documentTypeId)
        {
            var model = (await this.catalogService.GetDocumentTypes())?.FirstOrDefault(item => item.Id == documentTypeId);
            if(model == null)
            {
                return RedirectToAction("CreateDocumentType");
            }

            return View("DocumentTypeEdit", model);
        }
        
        [HttpPost("document-types/{documentTypeId}")]
        public async Task<IActionResult> UpdateDocumentType([FromRoute] int documentTypeId, DocumentType model)
        {
            try
            {
                await this.catalogService.UpdateDocumentType(documentTypeId, model.Name);
            }
            catch(Exception err)
            {
                ViewBag.ErrorMessage = err.Message;
                return View("DocumentTypeEdit", model);
            }
            return RedirectToAction("DocumentTypes");
        }

        #endregion
    }
}