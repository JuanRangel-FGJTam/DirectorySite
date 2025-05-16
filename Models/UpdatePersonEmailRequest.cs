using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DirectorySite.Models
{
    public class UpdatePersonEmailRequest
    {
        public string? PersonId { get; set; }
        
        [Display(Name = "Correo")]
        public string? Email { get; set; }

    }
}