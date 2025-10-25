using N3O.Umbraco.EditorJs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.EditorJs.DataTypes;

public class EditorJsBlockJsonConverter : JsonConverter {
    private readonly IEnumerable<IBlockDataConverter> _converters;

    public EditorJsBlockJsonConverter(IEnumerable<IBlockDataConverter> converters) {
        _converters = converters;
    }

    public override bool CanConvert(Type objectType) {
        return objectType.IsAssignableTo(typeof(EditorJsBlock));
    }

    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        var jObject = JObject.Load(reader);
        var id = (string) jObject["id"];
        var typeId = (string) jObject["type"];

        var converter = _converters.SingleOrDefault(x => x.CanConvert(typeId));

        if (converter == null) {
            throw new JsonException($"No converter found for type {typeId}");
        }
        
        var editorJsBlock = converter.Convert(id, typeId, serializer, (JObject) jObject["data"], (JObject) jObject["tunes"]);

        return editorJsBlock;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }
}