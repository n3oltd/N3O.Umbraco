using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class IndividualRes : IIndividual {
    public NameRes Name { get; set;}

    [JsonIgnore]
    IName IIndividual.Name => Name;
}