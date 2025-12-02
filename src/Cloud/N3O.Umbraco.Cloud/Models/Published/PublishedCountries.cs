using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedCountries : Value {
    [JsonProperty("countries")]
    public Dictionary<string, PublishedCountry> Countries { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Countries;
    }
}