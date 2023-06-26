using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiContactReq {
    [JsonConstructor]
    public ApiContactReq() { }
    
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("custom_fields")]
    public Dictionary<string, object> CustomFields { get; set; }
    
    [JsonExtensionData]
    public Dictionary<string, object> ReservedFields { get; set; }
}
