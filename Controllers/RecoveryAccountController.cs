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

        [HttpGet("{recordID}")]
        public async Task<IActionResult> Show(string recordID)
        {
            try
            {
                var record = await this.recoveryAccountService.GetRequestById(recordID);
                ViewData["Title"] = $"Peticion {recordID}";
                return View(record);
            }
            catch (KeyNotFoundException)
            {
                ViewData["Title"] = $"Peticion no encontrada";
                ViewBag.NotFoundMessage = "No se encontro ninguna solicitud activa con este id.";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Fal at attempt to retrive the request info: {message}", e.Message);
                var vm = new ErrorViewModel
                {
                    Message = "Error al obtener la peticion"
                };
                return View("~/Views/Shared/Error.cshtml", vm);
            }
        }


        [HttpPatch("{recordID}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> FinishRequest([FromRoute] string recordID, [FromForm] string comments, [FromForm] bool notifyEmail = false)
        {
            RecoveryAccountResponse recoveryRecord;
            try
            {
                recoveryRecord = await this.recoveryAccountService.GetRequestById(recordID);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Conflict();
            }

            // * update the request
            try 
            {
                await this.recoveryAccountService.UpdateTheRequest(recordID, comments, notifyEmail);
                return RedirectToAction("index", "RecoveryAccount");

            }catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{recordID}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DeleteRequest([FromRoute] string recordID, [FromForm] string comments, [FromForm] bool notifyEmail = false)
        {
            RecoveryAccountResponse recoveryRecord;
            try
            {
                recoveryRecord = await this.recoveryAccountService.GetRequestById(recordID);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Conflict();
            }

             // * update the request
            try 
            {
                await this.recoveryAccountService.DeleteTheRequest(recordID, comments, notifyEmail);
                return RedirectToAction("index", "RecoveryAccount");
            }
            catch(Exception)
            {
                return BadRequest();
            }
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
