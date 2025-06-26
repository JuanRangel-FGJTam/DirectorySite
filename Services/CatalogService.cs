using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using DirectorySite.Models;
using Newtonsoft.Json.Linq;

namespace DirectorySite.Services
{
    public class CatalogService(ILogger<CatalogService> logger,  IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        #region Fields
        private readonly ILogger<CatalogService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private string AuthToken {
            get {
                return httpContextAccessor.HttpContext!.Session.GetString("JWTToken")!;
            }
        }
        #endregion

        public async Task<IEnumerable<Occupation>?> GetOccupations()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/occupations"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Occupation>>();
                return data;
            }

            this.logger.LogError($"(-) Error at get catalog of occupations {httpResponse.StatusCode}\nToken:[{AuthToken}]");
            return null;
        }

        public async Task<IEnumerable<Gender>?> GetGenders()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/genders"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Gender>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of occupations " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<Nationality>?> GetNationalities()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/nationalities"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Nationality>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of Nationalities " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<MaritalStatus>?> GetMaritalStatuses()
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/marital-statuses"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<MaritalStatus>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of MaritalStatus " + httpResponse.StatusCode );
            return null;
        }

        public async Task<IEnumerable<ContactType>?> GetContactTypes()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/contact-types"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<ContactType>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of ContactTypes " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<Country>?> GetCountries()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/countries"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Country>>();
                return data;
            }

            this.logger.LogError("Error at get catalog of Countries " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<State>?> GetStates(int countryId = 0, string? search = null)
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var args = new string[]
            {
                $"country_id={countryId}",
                $"search={search}"
            };
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/states?" + string.Join("&", args)),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<State>>();
                return data;
            }

            this.logger.LogError("Error at get catalog of States " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<Municipality>?> GetMunicipalities(int stateId = 0, string? search = null)
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var args = new string[]
            {
                $"state_id={stateId}",
                $"search={search}"
            };
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/municipalities?" + string.Join("&", args)),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
                return data;
            }

            this.logger.LogError("Fail at get catalog of Municipality " + httpResponse.StatusCode );
            return null;
        }

        public async Task<IEnumerable<Colony>?> GetColonies(int municipalityId = 0, string? search = null)
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var args = new string[]
            {
                $"municipality_id={municipalityId}",
                $"search={search}"
            };
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/colonies?" + string.Join("&", args)),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Colony>>();
                return data;
            }

            this.logger.LogError("Fail at get the catalog of colonies " + httpResponse.StatusCode );
            return null;
        }

        public async Task<int> UpdateColony(int colonyId, string name, string? zipCode)
        {
            var payload = new
            {
                Name = name,
                ZipCode = zipCode
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/colonies/" + colonyId),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );
            if (httpResponse.IsSuccessStatusCode)
            {
                return 0;
            }

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());
                var errorsMessageList = GetAllValues(responseMessage!.errors);
                var errorMessage = string.Join(",", errorsMessageList);
                throw new Exception(errorMessage);
            }
            
            this.logger.LogError("Fail at get the catalog of colonies {statusCode}", httpResponse.StatusCode );
            httpResponse.EnsureSuccessStatusCode();
            return 1;
        }

        public async Task<IEnumerable<Role>?> GetRoles()
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/user/roles-availables"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken);

            var httpResponse = await httpClient.SendAsync(httpRequest);

            if (httpResponse.IsSuccessStatusCode)
            {
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Role>>();
                return data;
            }

            this.logger.LogError("Error at get catalog of user roles " + httpResponse.StatusCode);
            return null;
        }
    
        public async Task<int> StoreNewState(int countryId, string name)
        {
            var payload = new {
                CountryId = countryId,
                Name = name
            };
            
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/states"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            return 1;
            
        }

        public async Task<int> UpdateState(int stateId, string name)
        {
            var payload = new
            {
                Name = name
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/states/" + stateId),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );
            if (httpResponse.IsSuccessStatusCode)
            {
                return 0;
            }

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());
                var errorsMessageList = GetAllValues(responseMessage!.errors);
                var errorMessage = string.Join(",", errorsMessageList);
                throw new Exception(errorMessage);
            }
            
            this.logger.LogError("Fail at get the catalog of colonies {statusCode}", httpResponse.StatusCode );
            httpResponse.EnsureSuccessStatusCode();
            return 1;
        }

        public async Task<int> StoreNewMunicipality(int countryId, int stateId, string name)
        {
            var payload = new
            {
                CountryId = countryId,
                StateId = stateId,
                Name = name
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/municipalities"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken);
            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            return 1;

        }

        public async Task<int> UpdateMunicipality(int municipalityId, string name)
        {
            var payload = new
            {
                Name = name
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/municipalities/" + municipalityId),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );
            if (httpResponse.IsSuccessStatusCode)
            {
                return 0;
            }

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());
                var errorsMessageList = GetAllValues(responseMessage!.errors);
                var errorMessage = string.Join(",", errorsMessageList);
                throw new Exception(errorMessage);
            }
            
            this.logger.LogError("Fail at get the catalog of colonies {statusCode}", httpResponse.StatusCode );
            httpResponse.EnsureSuccessStatusCode();
            return 1;
        }

        public async Task<int> StoreNewColony(int countryId, int stateId, int municipalityId, string name, int zipCode)
        {
            var payload = new
            {
                CountryId = countryId,
                StateId = stateId,
                MunicipalityId = municipalityId,
                Name = name,
                ZipCode = zipCode.ToString()
            };

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/colonies"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken);
            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            return 1;
        }

        
        /// <summary>
        ///  search zipcode on the system
        /// </summary>
        /// <param name="zipcode">zipcode to search</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">The zipcode was not found</exception>
        public async Task<SearchZipcodeResponse> SearchZipcode(int zipcode)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken );
            var httpResponse = await httpClient.GetAsync($"/api/zipcode/search?zipcode={zipcode}");

            // * validate response
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("No hay resultados para este codigo postal.");
            }

            if( httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                httpResponse.EnsureSuccessStatusCode();
            }

            // * process the response
            var response = await httpResponse.Content.ReadFromJsonAsync<SearchZipcodeResponse>();
            return response ?? throw new Exception("Error al buscar el codigo postal.");
        }


        public async Task<IEnumerable<DocumentType>?> GetDocumentTypes()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/document-types"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<DocumentType>>();
                return data;
            }

            this.logger.LogError("Error at get catalog of ContactTypes {code}", httpResponse.StatusCode);
            return null;
        }

        public async Task DeleteDocumentType(int documentTypeId)
        {
            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/document-types/{documentTypeId}"),
                Method = HttpMethod.Delete
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            
            var httpResponse = await httpClient.SendAsync(httpRequest);
            if(httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            // * process the response
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("El tipo de documento no existe.");
            }

            // * attempt to cast the response
            var errorMessage = "No se puede eliminar el tipo de documento.";
            try
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());
                var errorsMessageList = GetAllValues(responseMessage!.errors);
                errorMessage = string.Join(",", errorsMessageList);
            }
            catch (System.Exception)
            {
                errorMessage = "No se puede eliminar el tipo de documento.";
            }
            throw new Exception(errorMessage);
        }

        public async Task StoreDocumentType(string name)
        {
            var payload = new {
                Name = name
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/document-types"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            var httpResponse = await httpClient.SendAsync(httpRequest);
            if(httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            // * attempt to cast the response
            var errorMessage = "No se puede registrar el tipo de documento.";
            try
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());
                var errorsMessageList = GetAllValues(responseMessage!.errors);
                errorMessage = string.Join(",", errorsMessageList);
            }
            catch (System.Exception)
            {
                errorMessage = "No se puede registrar el tipo de documento.";
            }
            throw new Exception(errorMessage);

        }

        public async Task UpdateDocumentType(int documentTypeId, string name)
        {
            var payload = new {
                Name = name
            };

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, $"/api/catalog/document-types/{documentTypeId}"),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + AuthToken );
            var httpResponse = await httpClient.SendAsync(httpRequest);
            if(httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            // * attempt to cast the response
            try
            {
                var responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>( await httpResponse.Content.ReadAsStringAsync());
                var errorsMessage = (IDictionary<string, string>) responseMessage!.errror;
                throw new Exception( string.Join(",", errorsMessage.Values) );
            }
            catch (System.Exception)
            {
                throw new Exception("No se puede actualizar el tipo de documento");
            }
        }


        static List<string> GetAllValues(JToken token)
        {
            List<string> values = new List<string>();

            // Recursively process the JToken
            if (token is JObject jObject)
            {
                foreach (var property in jObject.Properties())
                {
                    values.AddRange(GetAllValues(property.Value));
                }
            }
            else if (token is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    values.AddRange(GetAllValues(item));
                }
            }
            else
            {
                // Add primitive values (like string, number, etc.)
                values.Add(token.ToString());
            }

            return values;
        }
    }
}