using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DirectorySite.Models;
using Microsoft.AspNetCore.Authentication;

namespace DirectorySite.Services
{
    public class PeopleSearchService( ILogger<PeopleSearchService> logger,  IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor )
    {
        private readonly ILogger<PeopleSearchService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        

        public async Task<IEnumerable<SearchPersonResponse>?> SearchPerson(string search){
            
            // * Retrive the auth token for sending throw the api request
            var authToken = httpContextAccessor.HttpContext!.Session.GetString("JWTToken")!;
            

            // * Prepare the request
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/search-people?search={search}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            

            var httpResponse = await httpClient.SendAsync( httpRequest );
            
            // * Proccess the response
            IEnumerable<SearchPersonResponse>? responseData = null;
            if( httpResponse.IsSuccessStatusCode ){
                var text = await httpResponse.Content.ReadAsStringAsync();
                this.logger.LogDebug("Response:[{response}]", text);
                responseData = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<SearchPersonResponse>>();
            }

            return responseData;
        }

    }
}