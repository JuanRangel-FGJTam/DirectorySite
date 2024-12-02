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
    public class RecoveryAccountController(ILogger<RecoveryAccountController> logger, RecoveryAccountService recoveryAccountService ) : Controller
    {
        private readonly ILogger<RecoveryAccountController> _logger = logger;
        private readonly RecoveryAccountService recoveryAccountService = recoveryAccountService;

        public IActionResult Index()
        {
            ViewData["Title"] = "Peticiones de recuperacion de cuentas";
            return View();
        }

        #region Partial views
        [Route("table-records")]
        public async Task<IActionResult> GetTableRecords(){
            try
            {
                IEnumerable<RecoveryAccountResponse> responseData = await recoveryAccountService.GetRequest();
                return PartialView("~/Views/RecoveryAccount/Partials/RecordsTable.cshtml", responseData);
            }
            catch(Exception err)
            {
                this._logger.LogError(err,"Fail at retrive the records");
                var errorViewModel = new ErrorViewModel {
                    RequestId = "Fail at retrive the data"
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        #endregion
    }
}
