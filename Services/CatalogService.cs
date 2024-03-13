using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DirectorySite.Models;

namespace DirectorySite.Services
{
    public class CatalogService( ILogger<CatalogService> logger,  IHttpClientFactory httpClientFactory )
    {
        private readonly ILogger<CatalogService> logger = logger;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE3MTAzNTE2NjAsImV4cCI6MTcxODMwMDQ2MCwiaWF0IjoxNzEwMzUxNjYwfQ._bDpYYC-jXsKVl2ZE-K7jNjHZnQeFfLEON-ZHC-k6Zc";


        public async Task<IEnumerable<Occupation>?> GetOccupations()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/occupations"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Occupation>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of occupations " + httpResponse.StatusCode );
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
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
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
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
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
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
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
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
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
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Country>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of Countries " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<State>?> GetStates()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/states"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<State>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of States " + httpResponse.StatusCode );
            return null;
        }
        
        public async Task<IEnumerable<Municipality>?> GetMunicipalities()
        {

            var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress!, "/api/catalog/municipalities"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
            
            var httpResponse = await httpClient.SendAsync( httpRequest );

            if( httpResponse.IsSuccessStatusCode ){
                var data = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
                return data;
            }

            this.logger.LogError("(-) Error at get catalog of Municipality " + httpResponse.StatusCode );
            return null;
        }

        public async Task<IEnumerable<Occupation>?> GetColonies()
        {
           throw new NotImplementedException();
        }       
    }
}