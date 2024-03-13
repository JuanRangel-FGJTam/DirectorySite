using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DirectorySite.Models
{
    public class Country
    {

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("iso")]
        public string Iso { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phoneCode")]
        public int? PhoneCode { get; set; }
    }
}