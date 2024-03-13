using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DirectorySite.Controllers
{
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

    }
}