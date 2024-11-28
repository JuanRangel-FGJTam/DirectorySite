using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DirectorySite.Models
{
    public class UpdatePersonGeneralsRequest
    {
        public string? PersonId { get; set; }
        
        [Display(Name = "Nombre(s)")]
        public string? Name { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string? FirstName { get; set; }

        [Display(Name = "Apellido Materno")]
        public string? LastName { get; set; }

        [Display(Name = "CURP")]
        public string? Curp { get; set; }

        [Display(Name = "RFC")]
        public string? Rfc { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Genero")]
        public int? GenderId { get; set; }

        [Display(Name = "Estado Civil")]
        public int? MaritalStatusId { get; set; }

        [Display(Name = "Nacionalidad")]
        public int? NationalityId { get; set; }

        [Display(Name = "Ocupacion")]
        public int? OccupationId { get; set; }

        [JsonIgnore]
        public IEnumerable<SelectListItem> Genders { get; set; } = [];

        [JsonIgnore]
        public IEnumerable<SelectListItem> MaritalStatuses { get; set; } = [];

        [JsonIgnore]
        public IEnumerable<SelectListItem> Nationalities { get; set; } = [];

        [JsonIgnore]
        public IEnumerable<SelectListItem> Occupations { get; set; } = [];
    
    }
}