using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class ContactInformationResponse
    {
        [JsonPropertyName("contactId")]
        public string? ContactId { get; set; } = default!;

        [JsonPropertyName("contactTypeId")]
        public int ContactTypeId { get; set; }

        [JsonPropertyName("contactTypeName")]
        public string? ContactTypeName { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }