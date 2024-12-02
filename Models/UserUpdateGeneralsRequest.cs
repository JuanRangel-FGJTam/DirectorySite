using System;
using System.ComponentModel.DataAnnotations;

namespace DirectorySite.Models;

public class UserUpdateGeneralsRequest
{
    public int Id { get; set; }

    [Display(Name = "Nombre(s)")]
    public string? FirstName { get; set; }

    [Display(Name = "Apellidos")]
    public string? LastName { get; set; }

    [Display(Name = "Correo")]
    public string? Email { get; set; }

}
