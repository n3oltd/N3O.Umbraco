using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class Attribution {
    public IEnumerable<AttributionDimension> Dimensions { get; set; }
}