using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DirectorySite.Models
{
    public class PersonResponse
    {
        [JsonPropertyName("personId")]
        public string? PersonId { get; set; }

        [JsonPropertyName("rfc")]
        public string? Rfc { get; set; }

        [JsonPropertyName("curp")]
        public string? Curp { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("birthdate")]
        public DateTime? Birthdate { get; set; }

        [JsonPropertyName("genderId")]
        public int? GenderId { get; set; }

        [JsonPropertyName("genderName")]
        public string? GenderName { get; set; }

        [JsonPropertyName("maritalStatusId")]
        public int? MaritalStatusId { get; set; }

        [JsonPropertyName("maritalStatusName")]
        public string? MaritalStatusName { get; set; }

        [JsonPropertyName("nationalityId")]
        public int? NationalityId { get; set; }

        [JsonPropertyName("nationalityName")]
        public string? NationalityName { get; set; }

        [JsonPropertyName("occupationId")]
        public int? OccupationId { get; set; }

        [JsonPropertyName("occupationName")]
        public string? OccupationName { get; set; }

        [JsonPropertyName("appName")]
        public string? AppName { get; set; }

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("birthdateFormated")]
        public string? BirthdateFormated { get; set; }
    
    }
}