using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiGetContactByEmailReq {
    [JsonProperty("emails")]
    public IEnumerable<string> Emails { get; set; }
}