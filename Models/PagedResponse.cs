using System;
using Newtonsoft.Json;

namespace DirectorySite.Models;

public class PagedResponse<T>
{
    [JsonProperty("items")]
    public IEnumerable<T> Items { get; set; } = [];

    [JsonProperty("totalItems")]
    public int TotalItems { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
}