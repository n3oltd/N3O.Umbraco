using Newtonsoft.Json;

namespace N3O.Umbraco.EditorJs.Models;

public class EditorJsBlock<T> : EditorJsBlock {
    public EditorJsBlock(string id, string type, T data) : base(id, type, data) { }

    [JsonIgnore]
    public new T Data => (T) base.Data;
}

public abstract class EditorJsBlock {
    protected EditorJsBlock(string id, string type, object data) {
        Id = id;
        Type = type;
        Data = data;
    }

    public string Id { get; }
    public string Type { get; }
    public object Data { get; }
}