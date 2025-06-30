using Konstrukt.Mapping;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.UIBuilder.ValueMappers;

public class EnumDropdownValueMapper<T> : KonstruktValueMapper where T : struct, Enum {
    public override object ModelToEditor(object input) {
        var vals = Enum.GetValues<T>();
        var inputStr = input?.ToString();
        var val = input.HasValue() ? inputStr.ToEnum<T>().GetValueOrThrow() : vals[0];

        return JsonConvert.SerializeObject(new[] { val.ToString() });
    }

    public override object EditorToModel(object input) {
        var inputStr = input?.ToString();
        var rawVal = input.HasValue() ? JsonConvert.DeserializeObject<string[]>(inputStr) : Array.Empty<string>();

        var vals = Enum.GetValues<T>();
        var val = rawVal.HasAny() ? rawVal[0].ToEnum<T>().GetValueOrThrow() : vals[0];

        return val.ToString();
    }
}
