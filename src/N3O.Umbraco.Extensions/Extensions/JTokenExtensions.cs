using N3O.Umbraco.Exceptions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions {
    public static class JTokenExtensions {
        public static object ConvertToObject(this JToken jToken) {
            if (jToken is JValue jValue) {
                return jValue.Value;
            } else if (jToken is JObject jObject) {
                var dictionary = new Dictionary<string, object>();

                foreach (var (key, value) in jObject) {
                    dictionary[key] = ConvertToObject(value);
                }

                return dictionary;
            } else if (jToken is JArray jArray) {
                return jArray.Select(ConvertToObject).ToList();
            } else {
                throw UnrecognisedValueException.For(jToken.Type);
            }
        }
    }
}