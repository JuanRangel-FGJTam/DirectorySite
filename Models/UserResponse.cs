using System;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class UserResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("userRoles")]
    public IEnumerable<UserRole>? UserRoles { get; set; }

    [JsonPropertyName("userClaims")]
    public IEnumerable<object>? UserClaims { get; set; }

    public string FullName
    {
        get => string.Join(" ", FirstName, LastName);
    }
}

public class UserRole
{
    [JsonPropertyName("key")]
    public Guid Key { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    
    [JsonPropertyName("role")]
    public Role? Role{ get; set; }
}