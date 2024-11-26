using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using DirectorySite.Models;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">The auth token is invalid</exception>
        /// <exception cref="ArgumentException">The request is invalid</exception>
        /// <exception cref="InvalidDataException">Some internal error happens</exception>
        public async Task<int> UpdatePerson(string personID, UpdatePersonGeneralsRequest request)
        {
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

            // * prepare the payload
            var jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };
            var payload = JsonConvert.SerializeObject(request, settings:jsonSettings );


            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/{personID}"),
                Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");

            // * send the request and validate the response
            var response = await httpClient.SendAsync(httpRequest);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException httpEx)
            {
                throw httpEx.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => new ArgumentException(httpEx.Message, httpEx),
                    System.Net.HttpStatusCode.Unauthorized => new UnauthorizedAccessException(),
                    _ => new InvalidDataException(),
                };
            }

            return 1;
        }
    }
}