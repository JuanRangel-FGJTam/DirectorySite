#pragma warning disable CS8766 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DirectorySite.Data.Contracts;

namespace DirectorySite.Models.ViewModel
{
    public class PeopleIndexViewModel
    {
        public string? Search {get;set;}

        public IEnumerable<SearchPersonResponse> People {get;set;} = [];
    }
}