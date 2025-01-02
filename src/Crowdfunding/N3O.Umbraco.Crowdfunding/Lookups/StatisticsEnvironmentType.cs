using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class StatisticsEnvironmentType : NamedLookup {
    public StatisticsEnvironmentType(string id, string name) : base(id, name) { }
}

public class EnvironmentTypes : StaticLookupsCollection<StatisticsEnvironmentType> {
    public static readonly StatisticsEnvironmentType Staging = new("staging", "Staging");
    public static readonly StatisticsEnvironmentType Production = new("production", "Production");
}