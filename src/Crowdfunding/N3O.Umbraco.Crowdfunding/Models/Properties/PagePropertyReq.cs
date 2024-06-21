using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class PagePropertyReq {
    [Name("Alias")]
    public string Alias { get; set; }
    
    [Name("Type")]
    public PropertyType Type { get; set; }
    
    [Name("Boolean")]
    public BooleanValueReq Boolean { get; set; }
    
    [Name("TextBox")]
    public TextBoxValueReq TextBox { get; set; }
    
    [JsonIgnore]
    public AutoProperty<ValueReq> Value => new(this, x => x?.Type == Type);
}