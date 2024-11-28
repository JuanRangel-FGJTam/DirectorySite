using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace DirectorySite.Controllers
{
    [Auth]
    [Route("[controller]")]
    public class PreregisterController(ILogger<PreregisterController> logger, PreregisterService preregisterService, PreregisterDataContext preregisterDataContext) : Controller
    {
        private readonly ILogger<PreregisterController> _logger = logger;
        private readonly PreregisterService preregisterService = preregisterService;
        private readonly PreregisterDataContext preregisterDataContext = preregisterDataContext;


        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<PreregisterResponse> responseData = await preregisterService.GetPreregisterRecords();
                ViewBag.LastUpdate = preregisterDataContext.LastUpdate!.Value.ToString();
                return View(responseData);
            }
            catch(Exception err)
            {
                this._logger.LogError(err,"Fail at retrive the preregister records");
                var errorViewModel = new ErrorViewModel {
                    RequestId = "Fail at retrive the data"
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        [Route("{recordID}")]
        public async Task<IActionResult> ShowRecord([FromRoute] string recordID)
        {
            
            // * attempt to get the data
            PreregisterResponse? responseData = null;
            try
            {
                responseData = await preregisterService.GetPreregisterRecord(recordID);
            }
            catch(Exception err)
            {
                this._logger.LogError(err,"Fail at retrive the preregister records");
                var errorViewModel = new ErrorViewModel {
                    RequestId = "Fail at retrive the data"
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }

            if(responseData == null)
            {
                ViewData["ErrorMessage"]="El registro no se encuentra registrado en la base de datos";
            }

            // return the view
            return View(responseData);
        }

        [HttpPost("table-records/refresh")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ForceRefreshData()
        {
            try
            {
                await preregisterService.RefreshPreregisterRecords();    
                return Ok( new {
                    LastUpdate = this.preregisterDataContext.LastUpdate!.Value.ToString()
                });
            }
            catch (System.Exception)
            {
                return Conflict();
            }
        }

        #region Partial views
        [Route("table-records")]
        public async Task<IActionResult> GetTableRecords(){
            try
            {
                IEnumerable<PreregisterResponse> responseData = await preregisterService.GetPreregisterRecords();
                ViewBag.LastUpdate = preregisterDataContext.LastUpdate!.Value.ToString();
                return PartialView("~/Views/Preregister/Partials/RecordsTable.cshtml", responseData);
            }
            catch(Exception err)
            {
                this._logger.LogError(err,"Fail at retrive the preregister records");
                var errorViewModel = new ErrorViewModel {
                    RequestId = "Fail at retrive the data"
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        #endregion
    }
}
