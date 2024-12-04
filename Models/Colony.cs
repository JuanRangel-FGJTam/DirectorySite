using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DirectorySite.Models
{
    public class Colony
    {

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        
        [JsonPropertyName("zipCode")]
        public int? ZipCode { get; set; }
        
        [JsonPropertyName("municipality")]
        public Municipality? Municipality { get; set; }
    }
}