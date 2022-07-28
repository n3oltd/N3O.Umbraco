using Newtonsoft.Json;

namespace N3O.Umbraco.SerpEditor.Models;

public class SerpEntry {
    [JsonConstructor]
    public SerpEntry() { }

    public SerpEntry(SerpEntry other) {
        Title = other.Title;
        Description = other.Description;
    }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
}
