using Newtonsoft.Json;

namespace N3O.Umbraco.Uploader.DataTypes;

public class UploaderSource {
    [JsonProperty("altText")]
    public string AltText { get; set; }
    
    [JsonProperty("extension")]
    public string Extension { get; set; }
    
    [JsonProperty("filename")]
    public string Filename { get; set; }
    
    [JsonProperty("sizeMb")]
    public double SizeMb { get; set; }
    
    [JsonProperty("urlPath")]
    public string UrlPath { get; set; }
}
