using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DirectorySite.Models.ViewModel;

public class CatalogViewModel
{
    public int SelectedItemId { get; set; } = 0;
    public string? SearchText { get; set; }

    public IEnumerable<ICatalogViewModelItem> Data = [];

    public IEnumerable<SelectListItem> DataCatalog
    {
        get => Data.Select(item => new SelectListItem(item.Name, item.Id.ToString()));
    }

    public string? SelectedItemName
    {
        get => Data.FirstOrDefault(item => item.Id == SelectedItemId)?.Name;
    }
}

public interface ICatalogViewModelItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}