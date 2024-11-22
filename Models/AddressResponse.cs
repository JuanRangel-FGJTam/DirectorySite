using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class AddressResponse
    {
        [JsonPropertyName("addressId")]
        public string AddressId { get; set; } = default!;

        [JsonPropertyName("countryId")]
        public int CountryId { get; set; }

        [JsonPropertyName("countryName")]
        public string? CountryName { get; set; }

        [JsonPropertyName("stateId")]
        public int StateId { get; set; }

        [JsonPropertyName("stateName")]
        public string? StateName { get; set; }

        [JsonPropertyName("municipalityId")]
        public int MunicipalityId { get; set; }

        [JsonPropertyName("municipalityName")]
        public string? MunicipalityName { get; set; }

        [JsonPropertyName("colonyId")]
        public int ColonyId { get; set; }

        [JsonPropertyName("colonyName")]
        public string? ColonyName { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("numberInside")]
        public string? NumberInside { get; set; }

        [JsonPropertyName("zipCode")]
        public int ZipCode { get; set; }
    }