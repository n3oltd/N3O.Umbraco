using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiSearchContactsReq {
    [JsonProperty("query")]
    public string Query { get; set; }
}