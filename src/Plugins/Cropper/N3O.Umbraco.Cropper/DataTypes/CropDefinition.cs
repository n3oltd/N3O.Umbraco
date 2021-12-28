using Newtonsoft.Json;

namespace N3O.Umbraco.Cropper.DataTypes {
    public class CropDefinition {
        [JsonProperty("label")]
        public string Label { get; set; }
    
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    
        [JsonProperty("height")]
        public int Height { get; set; }
    
        [JsonProperty("filters")]
        public string Filters { get; set; }
    }
}
