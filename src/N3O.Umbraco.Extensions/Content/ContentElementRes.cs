using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public class ContentElementRes {
    public ContentElementRes() {
        Properties = new Dictionary<string, object>();
    }

    public Guid Key { get; set; }
    public string ContentTypeAlias { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> Properties { get; set; }
}