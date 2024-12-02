using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DirectorySite.Models
{
    public class UpdateRolesRequest
    {
        
        public List<int> Roles {get;set;} = new();

        [JsonIgnore]
        public IEnumerable<SelectListItem> RolesAvailables { get; set; } = [];
    
    }
}