using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DirectorySite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Abstractions;

namespace DirectorySite.Services
{
    public class PeopleService( ILogger<PeopleService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<PeopleService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        

        /// <summary>
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<PersonResponse?> GetPersonById(string personID){

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


            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/{personID}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");
            
            
            // * send the request
            PersonResponse? personResponse = null;
            try {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                
                // * proccess the response
                if( httpResponse.IsSuccessStatusCode ){
                    var text = await httpResponse.Content.ReadAsStringAsync();
                    this.logger.LogDebug("Response:[{response}]", text);
                    personResponse = await httpResponse.Content.ReadFromJsonAsync<PersonResponse>();
                }else {
                    // * handle status codes
                }
            }catch(Exception err){
                this.logger.LogError(err, "Fail at get the data of the person '{personId}'", personID);
            }

            return personResponse;
        }

    }
}