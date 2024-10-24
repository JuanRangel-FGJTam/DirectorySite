using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Data;
using DirectorySite.Services;
using Microsoft.AspNetCore.Mvc;


namespace DirectorySite.Controllers
{
    [Auth]
    [Route("[controller]")]
    public class CatalogController(ILogger<CatalogController> logger, CatalogService catalogService) : Controller
    {
        #region Fields
        private readonly ILogger<CatalogController> _logger = logger;
        private readonly CatalogService catalogService = catalogService ;
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