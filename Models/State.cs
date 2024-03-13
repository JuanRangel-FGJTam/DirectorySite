using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DirectorySite.Models
{
    public class State
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("country")]
        public Country? Country { get; set; }
    }
}