using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Data;
using DirectorySite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DirectorySite.Controllers
{

    [Auth]
    [Route("[controller]")]
    public class PeopleController(ILogger<PeopleController> logger, IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly ILogger<PeopleController> _logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index()
        {
            // Retrive the auth token
            var AuthToken = HttpContext.Session.GetString("JWTToken")!;
            
            // Prepare the request
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            // Proccess the response
            var httpResponse = await httpClient.SendAsync( httpRequest );
            IEnumerable<PersonResponse>? responseData = null;
            if( httpResponse.IsSuccessStatusCode ){
                responseData = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PersonResponse>>();
            }

            return View( responseData );
        }

    }
}