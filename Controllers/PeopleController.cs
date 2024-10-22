using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Data;
using DirectorySite.Models;
using DirectorySite.Services;

namespace DirectorySite.Controllers
{

    [Auth]
    [Route("[controller]")]
    public class PeopleController(ILogger<PeopleController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, PeopleSearchService _peopleSearchService) : Controller
    {
        private readonly ILogger<PeopleController> _logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IConfiguration configuration = configuration;
        private readonly PeopleSearchService peopleSearchService = _peopleSearchService;

        public IActionResult Index()
        {
            // Retrive the auth token
            var AuthToken = HttpContext.Session.GetString("JWTToken")!;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string? search)
        {

            ViewBag.SearchText = search;

            if( string.IsNullOrEmpty(search)){
                return View();
            }

            try {
                var peopleSearched = await this.peopleSearchService.SearchPerson(search!);
                return View(peopleSearched);

            }catch(Exception err){
                this._logger.LogError(err, "Fail at search the person");
                ViewBag.ErrorMessage = "Error al realizar la busqueda, " + err.Message;
                return View();
            }
        }

    }
}