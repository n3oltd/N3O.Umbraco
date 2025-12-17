using N3O.Umbraco.EditorJs.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.EditorJs.Models;

public class EditorJsBlock<TData> : EditorJsBlock {
    public EditorJsBlock(string id, string type, TData data, JObject tunesData) : base(id, type, data, tunesData) { }

    [JsonIgnore]
    public new TData Data => (TData) base.Data;
}

public abstract class EditorJsBlock {
    protected EditorJsBlock(string id, string type, object data, JObject tunesData) {
        Id = id;
        Type = type;
        Data = data;
        TunesData = tunesData;
    }

    public string Id { get; }
    public string Type { get; }
    public object Data { get; }
    public JObject TunesData { get; }

    public TTune GetTune<TTune>() where TTune : class {
        var tuneId = typeof(TTune).GetTuneId();

        if (TunesData?.TryGetValue(tuneId, out var tune) == true) {
            return tune.ToObject<TTune>();
        } else {
            return null;
        }
    }
}