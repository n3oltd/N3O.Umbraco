using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiFieldDefinitions {
    [JsonProperty("custom_fields")]
    public IEnumerable<ApiFieldDefinition> CustomFields { get; set; }
    
    [JsonProperty("reserved_fields")]
    public IEnumerable<ApiFieldDefinition> ReservedFields { get; set; }
}