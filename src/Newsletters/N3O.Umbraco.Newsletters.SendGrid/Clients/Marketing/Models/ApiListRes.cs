using N3O.Umbraco.Newsletters.SendGrid.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiListRes : ISendGridList {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}