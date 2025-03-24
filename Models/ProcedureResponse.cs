using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DirectorySite.Models
{

    public class ProcedureResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("peopleId")]
        public string? PeopleId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("folio")]
        public string? Folio { get; set; }

        [JsonPropertyName("denunciaId")]
        public string? DenunciaId { get; set; }

        [JsonPropertyName("observations")]
        public string? Observations { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }

        [JsonPropertyName("area")]
        public string? Area { get; set; }

        [JsonPropertyName("areaId")]
        public int AreaId { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("files")]
        public List<object>? Files { get; set; }

        [JsonPropertyName("officeLocation")]
        public string? OfficeLocation { get; set; }
    }
}