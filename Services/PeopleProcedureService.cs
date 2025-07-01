using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using DirectorySite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Abstractions;

namespace DirectorySite.Services
{
    public class PeopleProcedureService( ILogger<PeopleProcedureService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<PeopleProcedureService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        

        /// <summary>
        /// get the procedures registered of the person
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The request is not valid</exception>
        /// <exception cref="KeyNotFoundException">The person is not found</exception>
        /// <exception cref="UnauthorizedAccessException">The auth token is not valid or is missing</exception>
        /// <exception cref="Exception"> Fail at get the data</exception>
        public async Task<PagedResponse<ProcedureResponse>> GetProceduresOfPerson(string personID, int take = 25, int skip = 0){
            
            // * get the auth token
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
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/{personID}/procedures?take={take}&offset={skip}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            // * attempt to get the procedures data
            PagedResponse<ProcedureResponse> responseData = default!;
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                responseData = await httpResponse.Content.ReadFromJsonAsync<PagedResponse<ProcedureResponse>>()
                    ?? throw new Exception("Failed to deserialize the procedures response data.");
            }
            catch(HttpRequestException httpe)
            {
                this.logger.LogError(httpe, "Fail at retrive the procedures of the person '{personId}': {message}", personID, httpe.Message);
                throw httpe.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException("The request is not valid", httpe),
                    HttpStatusCode.NotFound => new KeyNotFoundException("The person was not found on the system", httpe),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException("Access token missing or not valid", httpe),
                    _ => new Exception("Error at retrive the data", httpe),
                };
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at retrive the procedures of the person '{personId}': {message}", personID, err.Message);
                throw new Exception("Error at retrive the data", err);
            }

            return responseData!;
        }

    }
}