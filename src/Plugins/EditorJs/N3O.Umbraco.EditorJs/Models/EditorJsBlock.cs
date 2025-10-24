using Newtonsoft.Json;

namespace N3O.Umbraco.EditorJs.Models;

public class EditorJsBlock<TData, TTunes> : EditorJsBlock {
    public EditorJsBlock(string id, string type, TData data, TTunes tunes) : base(id, type, data, tunes) { }

    [JsonIgnore]
    public new TData Data => (TData) base.Data;
    
    [JsonIgnore]
    public new TTunes Tunes => (TTunes) base.Tunes;
}

public abstract class EditorJsBlock {
    protected EditorJsBlock(string id, string type, object data, object tunes) {
        Id = id;
        Type = type;
        Data = data;
        Tunes = tunes;
    }

    public string Id { get; }
    public string Type { get; }
    public object Data { get; }
    public object Tunes { get; }
}