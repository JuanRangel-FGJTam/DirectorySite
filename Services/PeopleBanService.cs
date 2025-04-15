using System;
using System.Net.Cache;
using System.Net.Http.Headers;
using DirectorySite.Interfaces;
using DirectorySite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DirectorySite.Services;

public class PeopleBanService : IPeopleBanService
{
    private readonly ILogger<PeopleBanService> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly JsonSerializerSettings jsonSerializerSettings;
    private string authToken = string.Empty;

    public PeopleBanService(ILogger<PeopleBanService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.httpContextAccessor = httpContextAccessor;
        this.jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>A list of people banned</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public async Task<PagedResponse<PersonResponse>> GetPeopleBanned(int page, int pageSize)
    {
        LoadAuthToken();
        
        using var client = this.httpClientFactory.CreateClient("DirectoryAPI");
        
        // * prepare the request
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(client.BaseAddress!, $"/api/people-banned?take={pageSize}&offset={page * pageSize}")
        };
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        // * send the request
        var httpResponse = await client.SendAsync(httpRequest);
        try
        {
            httpResponse.EnsureSuccessStatusCode();
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

        // * process the response
        var response = await httpResponse.Content.ReadFromJsonAsync<PagedResponse<PersonResponse>>();
        if(response == null)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            this.logger.LogError("Fail at process the response [{response}]", responseString);
            throw new InvalidDataException("Can't process the reponse");
        }

        return response;
    }

    public async Task<IEnumerable<object>> GetPersonBanHistory()
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public Task BanPerson(Guid personId)
    {
        throw new NotImplementedException();
    }

    public async Task UnbanPerson(Guid personId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }


    private void LoadAuthToken()
    {
        // * get the Auth token
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