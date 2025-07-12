using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLookupsRoot : Value {
    [JsonProperty("lookups")]
    public PublishedLookups Lookups { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Lookups;
    }
}