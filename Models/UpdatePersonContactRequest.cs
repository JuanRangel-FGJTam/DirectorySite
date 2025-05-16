using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DirectorySite.Models
{
    public class UpdatePersonContactRequest
    {
        public string? PersonId { get; set; }

        public int ContactTypeId {get;set;}
        
        public string? Value { get; set; }

        [JsonIgnore]
        public IEnumerable<SelectListItem> ContactTypes { get; set; } = [];
    }
}