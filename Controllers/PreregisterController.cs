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
    public class PreregisterController(ILogger<PreregisterController> logger, PreregisterService preregisterService) : Controller
    {
        private readonly ILogger<PreregisterController> _logger = logger;
        private readonly PreregisterService preregisterService = preregisterService;
        

        public async Task<IActionResult> Index([FromQuery] int p = 1, [FromQuery] int filter = 0)
        {
            try
            {
                ViewBag.CurrentPage = p;
                ViewBag.CurrentFilterStatus = filter;
                var responseData = await preregisterService.GetPreregisters();
                ViewBag.LastUpdate = DateTime.Now;
                return View(responseData.Data);
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
                responseData = await preregisterService.GetPreregisterByID(recordID);
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

        #region Partial views
        [Route("table-records")]
        public async Task<IActionResult> GetTableRecords([FromQuery] int p = 1, [FromQuery] int filter = 0){
            try
            {
                int take = 25;
                int skip = (p-1) * take;

                // * get the data
                var preregisterPaginator = await this.preregisterService.GetPreregisters(take, skip);

                ViewBag.LastUpdate = DateTime.Now;
                ViewBag.TotalRecords = preregisterPaginator.Total;
                ViewBag.TotalPages = Math.Ceiling( (decimal) (preregisterPaginator.Total / take) + 1);
                ViewBag.CurrentPage = p;
                ViewBag.Skip = skip;

                return PartialView("~/Views/Preregister/Partials/RecordsTable.cshtml", preregisterPaginator.Data);
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
