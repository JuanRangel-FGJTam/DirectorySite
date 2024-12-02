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
    public class RecoveryAccountService(ILogger<RecoveryAccountService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<RecoveryAccountService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        private string authToken = string.Empty;

        /// <summary>
        /// return the las account recovery requests
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token or id invalid</exception>
        /// <exception cref="ArgumentException">The request is invalid</exception>
        public async Task<IEnumerable<RecoveryAccountResponse>> GetRequest()
        {
            // * load the authToken if is not loaded
            if(string.IsNullOrEmpty(authToken)){
                RetriveAuthToken();
            }

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/accountRecovery"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");
            
            // * send the request
            IEnumerable<RecoveryAccountResponse> requests = [];
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                requests = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<RecoveryAccountResponse>>() ?? [];
                return requests;
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