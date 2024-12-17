using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class RecoveryAccountPaginatorResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("data")]
    public List<RecoveryAccountResponse> Data { get; set; } = [];

    [JsonPropertyName("filters")]
    public RecoveryAccountFilters Filters { get; set; } = default!;

}

public class RecoveryAccountFilters
{
    [JsonPropertyName("take")]
    public int Take { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("orderBy")]
    public string OrderBy { get; set; } = default!;

    [JsonPropertyName("ascending")]
    public bool Ascending { get; set; }

    [JsonPropertyName("excludeConcluded")]
    public bool ExcludeConcluded { get; set; }

    [JsonPropertyName("excludeDeleted")]
    public bool ExcludeDeleted { get; set; }
}
