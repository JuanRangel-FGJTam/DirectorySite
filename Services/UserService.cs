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
    public class UserService( ILogger<UserService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<UserService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// get active users
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<IEnumerable<UserResponse>?> GetUsers()
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

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/user"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");

            // * send the request
            IEnumerable<UserResponse>? usersData = null;
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                // * process the response
                var text = await httpResponse.Content.ReadAsStringAsync();
                this.logger.LogDebug("Response:[{response}]", text);
                usersData = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();
            }
            catch(HttpRequestException httpex)
            {
                // TODO: Validate the http status code
                this.logger.LogError(httpex, "Fail at get the users: {message}", httpex.Message);
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at get the users: {message}", err.Message);
            }

            return usersData;
        }

        /// <summary>
        /// get the data of the user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<UserResponse?> GetUserData(int userId)
        {
            IEnumerable<UserResponse>? users = await GetUsers();
            if( users == null)
            {
                return null;
            }
            return users.FirstOrDefault(item => item.Id == userId);
        }
    
    }
}