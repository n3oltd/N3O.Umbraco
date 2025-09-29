using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.EditorJs.Models;

public class EditorJsModel {
    [JsonProperty("time")]
    public long LastModified { get; set; }
    
    [JsonProperty("blocks")]
    public IEnumerable<EditorJsBlock> Blocks { get; set; }
    
    [JsonProperty("version")]
    public string Version { get; set; }
}