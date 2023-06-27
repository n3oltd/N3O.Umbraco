using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiGetContactByEmailRes {
    [JsonProperty("result")]
    public Dictionary<string, ApiContactRes> Result { get; set; }
}