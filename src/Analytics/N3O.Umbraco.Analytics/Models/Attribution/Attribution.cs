using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Analytics.Models;

public class Attribution : Value {
    public Attribution(IEnumerable<AttributionDimension> dimensions) {
        Dimensions = dimensions.OrEmpty().ToList();
    }

    public IEnumerable<AttributionDimension> Dimensions { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        foreach (var dimension in Dimensions) {
            yield return dimension;
        }
    }
    
    public static readonly Attribution Empty = new(null);
}