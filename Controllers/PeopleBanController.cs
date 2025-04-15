using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;
using DirectorySite.Interfaces;


namespace DirectorySite.Controllers
{
    [Auth]
    [Route("[controller]")]
    public class PeopleBanController(ILogger<PeopleBanController> logger, IPeopleBanService peopleBanService) : Controller
    {
        private readonly ILogger<PeopleBanController> logger = logger;
        private readonly IPeopleBanService peopleBanService = peopleBanService;
        private readonly int pageSize = 25;
        
        [HttpGet("")]
        public ActionResult Index(int page = 1)
        {
            ViewBag.Title = "Personas bloqueadas";
            ViewBag.CurrentPage = page;
            return View();
        }

        #region Partial Views
        [Route("table-records")]
        public async Task<IActionResult> GetTableRecords([FromQuery] int page = 1)
        {
            try
            {
                PagedResponse<PersonResponse> response = await peopleBanService.GetPeopleBanned((page - 1), pageSize);

                // * prepare paginator
                ViewBag.LastUpdate = DateTime.Now;
                ViewBag.TotalRecords = response.TotalItems;
                ViewBag.TotalPages = Math.Ceiling( (decimal) (response.TotalItems / pageSize) + 1);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;

                return PartialView("~/Views/PeopleBan/Partials/PeopleTable.cshtml", response.Items);
            }
            catch(Exception err)
            {
                this.logger.LogError(err,"Fail at retrive the people banned records");
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Fail at retrive the data"
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }
        #endregion

    }
}
