using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DirectorySite.Models
{
    public class UpdatePersonGeneralsErrorResponse
    {
        public string? Title { get; set; }

        public IDictionary<string, IEnumerable<string>> Errors { get; set; } = default!;
    }
}