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
            
            // * attempt to get the sessions
            SessionsResponse? responseData = null;
            try {
                var httpResponse = await httpClient.SendAsync( httpRequest );
                httpResponse.EnsureSuccessStatusCode();
                responseData = await httpResponse.Content.ReadFromJsonAsync<SessionsResponse>();
            }catch(Exception err){
                this._logger.LogError(err,"Fail at retrive the sessions data");
            }

            return View( responseData );
        }

    }
}