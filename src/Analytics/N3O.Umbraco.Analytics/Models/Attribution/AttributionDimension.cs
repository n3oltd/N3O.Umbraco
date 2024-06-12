using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionDimension {
    public int Index { get; set; }
    public IEnumerable<AttributionOption> Options { get; set; }
}