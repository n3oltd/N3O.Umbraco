using N3O.Umbraco.Attributes;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class IndividualReq : IIndividual {
    [Name("Name")]
    public NameReq Name { get; set; }
    
    [JsonIgnore]
    IName IIndividual.Name => Name;
}