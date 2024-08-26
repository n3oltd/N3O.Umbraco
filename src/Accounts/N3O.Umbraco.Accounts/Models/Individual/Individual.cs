using Newtonsoft.Json;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Accounts.Models;

public class Individual : Value, IIndividual {
    [JsonConstructor]
    public Individual(Name name) {
        Name = name;
    }

    public Individual(IIndividual individual)
        : this(individual.Name.IfNotNull(x => new Name(x))) { }

    public Name Name { get; }
    
    [JsonIgnore]
    IName IIndividual.Name => Name;
}