using System;
using System.Collections;
using System.Collections.Generic;
using DirectorySite.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DirectorySite.Models.ViewModel;
public class CatalogStatesViewModel
{
    public int CountryId {get;set;} = 0;
    public int StateId {get;set;} = 0;
    public int MunicipalityId {get;set;} = 0;
    public string? SearchText { get; set; }
    
    public IEnumerable<Country> Countries = [];
    public IEnumerable<State> States = [];
    public IEnumerable<Municipality> Municipalities = [];
    public IEnumerable<Colony> Colonies = [];

    public IEnumerable<SelectListItem> CountriesCatalog
    {
        get => Countries.Select( item => new SelectListItem(item.Name, item.Id.ToString()));
    }
    public IEnumerable<SelectListItem> StateCatalog
    {
        get => States.Select( item => new SelectListItem(item.Name, item.Id.ToString()));
    }
    public IEnumerable<SelectListItem> MunicipalitiesCatalog
    {
        get => Municipalities.Select( item => new SelectListItem(item.Name, item.Id.ToString()));
    }

    public string? CountryName
    {
        get => Countries.FirstOrDefault( item => item.Id == CountryId)?.Name;
    }

    public string? StateName
    {
        get => States.FirstOrDefault( item => item.Id == StateId)?.Name;
    }

    public string? MunicipalityName
    {
        get => Municipalities.FirstOrDefault( item => item.Id == MunicipalityId)?.Name;
    }

}