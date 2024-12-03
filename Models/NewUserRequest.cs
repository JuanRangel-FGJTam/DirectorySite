using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class NewUserRequest
{
    [Display(Name = "Nombre(s)")]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Apellidos")]
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; } = null!;
    
    [Display(Name = "Correo")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [Display(Name = "Contraseña")]
    [JsonPropertyName("password")]
    public string? Password { get; set; } = null!;
    
    [Display(Name = "Confirmar Contraseña")]
    [JsonPropertyName("confirmPassword")]
    public string? ConfirmPassword { get; set; } = null!;
}