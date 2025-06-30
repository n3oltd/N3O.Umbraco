using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLookups {
    [JsonProperty("subscriptions.country")]
    public IEnumerable<PublishedCountry> Countries { get; set; }
}