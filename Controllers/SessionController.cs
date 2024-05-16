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
    public class SessionController(ILogger<SessionController> logger, IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly ILogger<SessionController> _logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index()
        {
            // Retrive the auth token
            var AuthToken = HttpContext.Session.GetString("JWTToken")!;
            var _take = 25;
            var _skip = 0;

            // Prepare the request
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/session?take={_take}&skip={_skip}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            // Proccess the response
            var httpResponse = await httpClient.SendAsync( httpRequest );
            SessionsResponse? responseData = null;
            if( httpResponse.IsSuccessStatusCode ){
                responseData = await httpResponse.Content.ReadFromJsonAsync<SessionsResponse>();
            }

            return View( responseData );
        }

    }
}