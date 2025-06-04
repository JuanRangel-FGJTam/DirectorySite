using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DirectorySite.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DirectorySite.Models.ViewModel;
public class NewCatalogStatesViewModel
{
    [Display(Name = "Pais")]
    [Required(ErrorMessage = "El pais es requerido")]
    public int? CountryId { get; set; }
    
    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El estado es requerido")]
    public int? StateId { get; set; }
    
    [Display(Name = "Municipio")]
    [Required(ErrorMessage = "El municipio es requerido")]
    public int? MunicipalityId { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El nombre del estado es requerido")]
    public string? StateName { get; set; }

    [Display(Name = "Municipio")]
    [Required(ErrorMessage = "El nombre del municipio es requerido")]
    public string? MunicipalityName { get; set; }

    [Display(Name = "Colonia")]
    [Required(ErrorMessage = "El nombre de la colonia es requerido")]
    public string? ColonyName { get; set; }

    [Display(Name = "Codigo Postal")]
    public string? ZipCode { get; set; }


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
}