using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiAddOrUpdateContactsReq {
    [JsonProperty("list_ids")]
    public IEnumerable<string> ListIds { get; set; }
    
    [JsonProperty("contacts")]
    public IEnumerable<ApiContactReq> Contacts { get; set; }
}