using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using DirectorySite.Models;
using DirectorySite.Exceptions;


namespace DirectorySite.Services
{
    public class UserService( ILogger<UserService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILogger<UserService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private string authToken = string.Empty;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// get active users
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<IEnumerable<UserResponse>?> GetUsers()
        {
            LoadAuthToken();

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

        /// <summary>
        /// get the data of the user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        public async Task<int> UpdateUserGenerals(int userId, UserUpdateGeneralsRequest request)
        {
            LoadAuthToken();

            // * prepare the payload
            var payload = JsonConvert.SerializeObject(request, jsonSerializerSettings);

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/user/{userId}"),
                Method = HttpMethod.Put,
                Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");

            // * send the request
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                // * process the response
                var text = await httpResponse.Content.ReadAsStringAsync();
                this.logger.LogDebug("Response:[{response}]", text);
                return 1;
            }
            catch(HttpRequestException httpex)
            {
                // TODO: Validate the http status code
                this.logger.LogError(httpex, "Fail at attempt to update the user: {message}", httpex.Message);
                return 0;
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at attempt to update the user: {message}", err.Message);
                return 0;
            }

        }

        /// <summary>
        /// get the data of the user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        /// <exception cref="ArgumentException">Some validations fails</exception>
        public async Task<int> UpdateCredentials(int userId, UserUpdateCredentialsRequest request)
        {
            LoadAuthToken();

            // * prepare the payload
            var payload = JsonConvert.SerializeObject(request, jsonSerializerSettings);

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/user/{userId}"),
                Method = HttpMethod.Put,
                Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");

            // * send the request
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();
                // * process the response
                var text = await httpResponse.Content.ReadAsStringAsync();
                this.logger.LogDebug("Response:[{response}]", text);
                return 1;
            }
            catch(HttpRequestException httpex)
            {
                if(httpex.StatusCode == System.Net.HttpStatusCode.UnprocessableContent)
                {
                    throw new ArgumentException("Las contraseñas no coinciden");
                }

                // TODO: Validate the http status code
                this.logger.LogError(httpex, "Fail at attempt to update the user: {message}", httpex.Message);
                return 0;
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at attempt to update the user: {message}", err.Message);
                return 0;
            }

        }


        /// <summary>
        /// </summary>
        /// <param name="NewUserRequest"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"> Fail at attempt to get the auth token</exception>
        /// <exception cref="ArgumentException">Some validations fails</exception>
        public async Task<int> StoreNewUser(NewUserRequest request)
        {
            LoadAuthToken();

            // * prepare the payload
            var payload = JsonConvert.SerializeObject(request, jsonSerializerSettings);

            // * prepare the request
            using var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/user"),
                Method = HttpMethod.Post,
                Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken}");

            // * send the request
            IDictionary<string,object>? responseObject = null;
            try
            {
                var httpResponse = await httpClient.SendAsync(httpRequest);
                // * process the response
                responseObject = await httpResponse.Content.ReadFromJsonAsync<IDictionary<string,object>>();
                this.logger.LogDebug("Response:[{response}]", responseObject);
                httpResponse.EnsureSuccessStatusCode();
                return 1;
            }
            catch(HttpRequestException httpex)
            {
                this.logger.LogError(httpex, "Fail at store the new user: {message}", httpex.Message);
                
                // TODO: Validate the http status code
                switch (httpex.StatusCode)
                {
                    case HttpStatusCode.UnprocessableEntity:
                        var errorMessages = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<IDictionary<string,object>>>(responseObject!["errors"].ToString()??"");
                        throw new ArgumentException(string.Join(", ", errorMessages!.Select( item => item["value"])));

                    case HttpStatusCode.BadRequest:
                        throw new ArgumentException(responseObject!["message"].ToString());
                    
                    default:
                        throw new ApplicationException( httpex.Message);
                }
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at attempt to store the user: {message}", err.Message);
                throw new ApplicationException(err.Message);
            }

        }

        #region private methods

        /// <summary>
        /// load the auth token
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private void LoadAuthToken()
        {
            try
            {
                authToken = httpContextAccessor.HttpContext!.Session.GetString("JWTToken")!;
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