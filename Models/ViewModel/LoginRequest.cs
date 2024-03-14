#pragma warning disable CS8766 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Data.Contracts;

namespace DirectorySite.Models.ViewModel
{
    public class LoginRequest : IUserCredentials
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string? Password {get;set;}

        public string? ErrorMessage {get;set;}
    }
}