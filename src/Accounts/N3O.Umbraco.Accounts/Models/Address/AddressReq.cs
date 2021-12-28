using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public class AddressReq : IAddress {
    [Name("Line 1")]
    public string Line1 { get; set; }
    
    [Name("Line 2")]
    public string Line2 { get; set; }
    
    [Name("Line 3")]
    public string Line3 { get; set; }
    
    [Name("Locality")]
    public string Locality { get; set; }
    
    [Name("Administrative Area")]
    public string AdministrativeArea { get; set; }
    
    [Name("Postal Code")]
    public string PostalCode { get; set; }
    
    [Name("Country")]
    public Country Country { get; set; }
}
