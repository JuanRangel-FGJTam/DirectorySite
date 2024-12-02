using System;
using System.ComponentModel.DataAnnotations;

namespace DirectorySite.Models;

public class UserUpdateCredentialsRequest
{
    public int Id { get; set; }

    [Display(Name = "Contraseña")]
    public string? Password { get; set; }

    [Display(Name = "Confirmar Contraseña")]
    public string? ConfirmPassword { get; set; }

}
