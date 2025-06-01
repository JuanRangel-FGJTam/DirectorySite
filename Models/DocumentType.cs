using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DirectorySite.Models.ViewModel;

namespace DirectorySite.Models
{
    public class DocumentType : ICatalogViewModelItem
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        int ICatalogViewModelItem.Id { get => this.Id ?? -1; set => Id = value; }
    }
}