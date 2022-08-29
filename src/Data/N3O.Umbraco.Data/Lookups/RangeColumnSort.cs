using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Lookups;

public class RangeColumnSort : NamedLookup {
    public RangeColumnSort(string id, string name, Action<List<string>> sort) : base(id, name) {
        Sort = sort;
    }

    public Action<List<string>> Sort { get; }
}

public class RangeColumnSorts : StaticLookupsCollection<RangeColumnSort> {
    public const string Preserve_Id = "preserve";
    public static readonly RangeColumnSort Preserve = new(Preserve_Id, "Preserve", _ => { });

    public const string Alphabetical_Id = "alphabetical";
    public static readonly RangeColumnSort Alphabetical = new("alphabetical", "Alphabetical", x => x.Sort(StringComparer.InvariantCultureIgnoreCase));

    public static RangeColumnSort GetById(string id) {
        if (id == Alphabetical_Id) {
            return Alphabetical;
        } else if (id == Preserve_Id) {
            return Preserve;
        } else {
            return null;
        }
    }
}
