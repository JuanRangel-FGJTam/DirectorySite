using System;

namespace DirectorySite.Models;

public class SearchZipcodeResponse
{
    public string Zipcode { get; set; } = default!;
    public IEnumerable<SearchZipcodeResponseCountry> Results { get; set; } = default!;
}


public class SearchZipcodeResponseItem
    {
        public int ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; } = default!;
        public int StateId { get; set; }
        public string State { get; set; } = default!;
        public int MunicipalityId { get; set; }
        public string Municipality { get; set; } = default!;
        public int ColonyId { get; set; }
        public string ColonyName { get; set; } = default!;
    }

    public class SearchZipcodeResponseCountry
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = default!;
        public IEnumerable<SearchZipcodeResponseState> States { get; set; } = default!;
    }

    public class SearchZipcodeResponseState
    {
        public int StateId { get; set; }
        public string StateName { get; set; } = default!;
        public IEnumerable<SearchZipcodeResponseItem> Data {get;set;} = default!;
    }
