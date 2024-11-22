using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using DirectorySite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Abstractions;

namespace DirectorySite.Services
{
    public class PeopleSessionService( ILogger<PeopleSessionService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<PeopleSessionService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        

        /// <summary>
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<SessionsResponse?> GetSessionsOfPerson(string personID, int take = 25, int skip = 0){

            string authToken = string.Empty;
            try
            {
                authToken = httpContextAccessor.HttpContext!.Session.GetString("JWTToken")!;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error at attempting to retrive the auth token: {message}", ex.Message);
                throw new UnauthorizedAccessException();
            }

            // Prepare the request
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/session?personId={personID}&take={take}&skip={skip}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            // * attempt to get the sessions
            SessionsResponse? responseData = null;
            try {
                var httpResponse = await httpClient.SendAsync( httpRequest );
                httpResponse.EnsureSuccessStatusCode();
                responseData = await httpResponse.Content.ReadFromJsonAsync<SessionsResponse>();
            }catch(Exception err){
                this.logger.LogError(err, "Fail at retrive the sessions data");
            }

            return responseData;
        }

    }
}