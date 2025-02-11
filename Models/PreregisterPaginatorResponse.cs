using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class PreregisterPaginatorResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("data")]
    public List<PreregisterResponse> Data { get; set; } = [];

    [JsonPropertyName("filters")]
    public PreregisterPaginatorFilters Filters { get; set; } = default!;

}

public class PreregisterPaginatorFilters
{
    [JsonPropertyName("take")]
    public int Take { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("orderBy")]
    public string OrderBy { get; set; } = default!;

    [JsonPropertyName("ascending")]
    public bool Ascending { get; set; }
}
