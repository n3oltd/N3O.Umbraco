using N3O.Umbraco.EditorJs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.EditorJs;

public interface IBlockDataConverter {
    bool CanConvert(string typeId);
    EditorJsBlock Convert(string id, string typeId, JsonSerializer serializer, JObject data);
}