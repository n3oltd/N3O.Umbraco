using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.SendGrid; 

public class ApiFieldDefinition {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("field_type")]
    public string FieldType { get; set; }
}