using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorySite.Models
{
    public class SessionsResponse
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Total { get; set; }
        public IEnumerable<SessionResponse>? Data { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SessionResponse
    {
        public string? SessionID { get; set; }
        public Person? Person { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime? BegginAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string? Token { get; set; }
    }

    public class Person
    {
        public string? Id { get; set; }
        public string? Rfc { get; set; }
        public string? Curp { get; set; }
        public string? Name { get; set; }
        public string? firstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? AppOwner { get; set; }
        public string? FullName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public object Gender { get; set; }
        public object MaritalStatus { get; set; }
        public object Bationality { get; set; }
        public object Occupation { get; set; }
        public object Addresses { get; set; }
        public object ContactInformations { get; set; }
        
    }


}