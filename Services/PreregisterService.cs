using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using DirectorySite.Data;
using DirectorySite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Abstractions;

namespace DirectorySite.Services
{
    public class PreregisterService(ILogger<PreregisterService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<PreregisterService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        private string authToken = string.Empty;


        public async Task<PreregisterPaginatorResponse> GetPreregisters(int take = 25, int skip = 0)
        {

            // * load the authToken if is not loaded
            if(string.IsNullOrEmpty(authToken)){
                RetriveAuthToken();
            }

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/pre-registration?take={take}&offset={skip}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");
            
            // * send the request
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                var tmpData = await httpResponse.Content.ReadFromJsonAsync<PreregisterPaginatorResponse>() ?? throw new Exception("Fail to parse the response");
                return tmpData;
            }
            catch(HttpRequestException httpex)
            {
                this.logger.LogError(httpex, "Fail at get the preregister records: {message}", httpex.Message);
                throw httpex.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException(),
                    HttpStatusCode.BadRequest => new ArgumentException(),
                    _ => new Exception(),
                };
            }
        }

        public async Task<PreregisterResponse?> GetPreregisterByID(string recordID)
        {
            // * load the authToken if is not loaded
            if(string.IsNullOrEmpty(authToken)){
                RetriveAuthToken();
            }

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/pre-registration/{recordID}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");
            
            // * send the request
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                var tmpData = await httpResponse.Content.ReadFromJsonAsync<PreregisterResponse>() ?? throw new Exception("Fail to parse the response");
                return tmpData;
            }
            catch(HttpRequestException httpex)
            {
                this.logger.LogError(httpex, "Fail at get the preregister records: {message}", httpex.Message);
                throw httpex.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException(),
                    HttpStatusCode.BadRequest => new ArgumentException(),
                    _ => new Exception(),
                };
            }
        }

        public async Task DeletePreregisterByID(string recordID)
        {
            // * load the authToken if is not loaded
            if(string.IsNullOrEmpty(authToken)){
                RetriveAuthToken();
            }

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/pre-registration/{recordID}"),
                Method = HttpMethod.Delete
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");
            
            // * send the request
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                return;
            }
            catch(HttpRequestException httpex)
            {
                this.logger.LogError(httpex, "Fail at get the preregister records: {message}", httpex.Message);
                throw httpex.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException(),
                    HttpStatusCode.BadRequest => new ArgumentException(),
                    _ => new Exception(),
                };
            }
        }

        #region private methods
        /// <summary>
        /// load the auth token
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private void RetriveAuthToken()
        {
            try
            {
                this.authToken = httpContextAccessor.HttpContext!.Session.GetString("JWTToken")!;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error at attempting to retrive the auth token: {message}", ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
        #endregion

    }
}