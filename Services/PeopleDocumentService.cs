using System;
using System.Collections.Generic;
using System.Net;
using DirectorySite.Core;
using DirectorySite.Models;
using Newtonsoft.Json;

namespace DirectorySite.Services;

public class PeopleDocumentService : IPeopleDocumentService
{
    private readonly ILogger<PeopleDocumentService> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IHttpContextAccessor httpContextAccessor;

    private string authToken = String.Empty;

    public PeopleDocumentService(ILogger<PeopleDocumentService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException">If the authentication token cannot be retrieved or is invalid</exception>
    /// <exception cref="ArgumentException">A parameter is missing</exception>
    /// <exception cref="KeyNotFoundException">The person dosent exist on the system</exception>
    public async Task<IEnumerable<PersonDocumentResponse>> GetPeopleDocuments(Guid personId)
    {
        this.logger.LogDebug("Starting to get documents for person with ID: {personId}", personId);

        LoadAuthToken();

        // * prepare the request
        var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/{personId}/documents"),
            Method = HttpMethod.Get
        };
        httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
        
        // * attempt to get the procedures data
        IEnumerable<PersonDocumentResponse>? responseData = null;
        try
        {
            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            var responseJsonString = await httpResponse.Content.ReadAsStringAsync();
            responseData = JsonConvert.DeserializeObject<IEnumerable<PersonDocumentResponse>>(responseJsonString);
        }
        catch(HttpRequestException httpe)
        {
            this.logger.LogError(httpe, "Fail at retrive the procedures of the person '{personId}': {message}", personId, httpe.Message);
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
            this.logger.LogError(err, "Fail at retrive the procedures of the person '{personId}': {message}", personId, err.Message);
            throw new Exception("Error at retrive the data", err);
        }

        return responseData ?? Array.Empty<PersonDocumentResponse>();
    }


    /// <summary>
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException">If the authentication token cannot be retrieved or is invalid</exception>
    /// <exception cref="ArgumentException">A parameter is missing</exception>
    /// <exception cref="KeyNotFoundException">The person dosent exist on the system</exception>
    public async Task<IEnumerable<PersonDocumentResponse>> GetPeopleDocumentsByType(Guid personId, int typeId)
    {
        this.logger.LogDebug("Starting to get documents for person with ID: {personId}", personId);

        LoadAuthToken();

        // * prepare the request
        var httpClient = httpClientFactory.CreateClient("DirectoryAPI");
        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(httpClient.BaseAddress!, $"/api/people/{personId}/documents/{typeId}"),
            Method = HttpMethod.Get
        };
        httpRequest.Headers.Add("Authorization", "Bearer " + authToken );
        
        // * attempt to get the procedures data
        IEnumerable<PersonDocumentResponse>? responseData = null;
        try
        {
            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            responseData = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PersonDocumentResponse>>();
        }
        catch(HttpRequestException httpe)
        {
            this.logger.LogError(httpe, "Fail at retrive the procedures of the person '{personId}': {message}", personId, httpe.Message);
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
            this.logger.LogError(err, "Fail at retrive the procedures of the person '{personId}': {message}", personId, err.Message);
            throw new Exception("Error at retrive the data", err);
        }

        return responseData ?? Array.Empty<PersonDocumentResponse>();
    }


    /// <summary>
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">if the authentication token cannot be retrieved.</exception>
    private void LoadAuthToken()
    {
        // * get the auth token
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
}
