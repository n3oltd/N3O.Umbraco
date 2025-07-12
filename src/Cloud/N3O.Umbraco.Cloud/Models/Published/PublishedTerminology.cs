using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedTerminology : Value {
    public string Campaigns { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Campaigns;
    }
}