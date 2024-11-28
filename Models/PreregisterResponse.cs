using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class PreregisterResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("mail")]
        public string? Mail { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("validTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }