using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedNisabAmount : Value {
    public decimal Gold { get; set; }
    public decimal Silver { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Gold;
        yield return Silver;
    }
}