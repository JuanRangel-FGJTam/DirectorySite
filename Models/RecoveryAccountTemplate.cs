using System;

namespace DirectorySite.Models;

public class RecoveryAccountTemplate
{
    public int Id { get; set; }
    public string Name {get;set;} = default!;
    public string Label {get;set;} = default!;
    public string? Template {get;set;}
}
