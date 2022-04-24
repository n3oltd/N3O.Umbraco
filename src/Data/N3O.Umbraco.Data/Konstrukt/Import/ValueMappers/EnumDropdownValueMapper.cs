using Konstrukt.Mapping;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Data.Konstrukt {
    public class EnumDropdownValueMapper<T> : KonstruktValueMapper where T : struct, Enum {
        public override object ModelToEditor(object input) {
            var vals = Enum.GetValues<T>();
            var inputStr = input?.ToString();
            var val = input.HasValue() ? (T) Enum.Parse(typeof(T), inputStr, true) : vals[0];

            return JsonConvert.SerializeObject(new[] { val.ToString() });
        }

        public override object EditorToModel(object input) {
            var inputStr = input?.ToString();
            var rawVal = input.HasValue() ? JsonConvert.DeserializeObject<string[]>(inputStr) : Array.Empty<string>();

            var vals = Enum.GetValues<T>();
            var val = rawVal.HasAny() ? (T) Enum.Parse(typeof(T), rawVal[0], true) : vals[0];

            return val.ToString();
        }
    }
}