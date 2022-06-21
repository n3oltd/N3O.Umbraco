using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Lookups;

public class LookupsCriteria {
    [Name("Types")]
    public IEnumerable<LookupInfo> Types { get; set; }
}
