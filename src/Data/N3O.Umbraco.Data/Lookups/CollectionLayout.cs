using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups;

public class CollectionLayout : NamedLookup {
    public CollectionLayout(string id, string name, bool multiColumn, string valueSeparator) : base(id, name) {
        MultiColumn = multiColumn;
        ValueSeparator = valueSeparator;
    }

    public bool MultiColumn { get; }
    public string ValueSeparator { get; }
}

public class CollectionLayouts : StaticLookupsCollection<CollectionLayout> {
    public const string CommaSeparated_Id = "commaSeparated";
    public static readonly CollectionLayout CommaSeparated = new(CommaSeparated_Id, "Comma Separate", false, ", ");

    public const string ValuePerColumn_Id = "valuePerColumn";
    public static readonly CollectionLayout ValuePerColumn = new(ValuePerColumn_Id, "Value Per Column", true, null);

    public static CollectionLayout GetById(string id) {
        if (id.EqualsInvariant(CommaSeparated_Id)) {
            return CommaSeparated;
        } else if (id.EqualsInvariant(ValuePerColumn_Id)) {
            return ValuePerColumn;
        } else {
            return null;
        }
    }
}
