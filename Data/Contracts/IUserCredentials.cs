using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorySite.Data.Contracts
{
    public interface IUserCredentials
    {
        public string Email {get;set;}
        public string Password {get;set;}
    }
}