using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiSearchContactsRes {
    [JsonProperty("result")]
    public ApiContactRes Result { get; set; }
    
    [JsonProperty("contact_count")]
    public int ContactCount { get; set; }
}