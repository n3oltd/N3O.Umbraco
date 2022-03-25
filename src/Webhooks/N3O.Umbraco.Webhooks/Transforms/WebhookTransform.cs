using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Webhooks.Transforms {
    public abstract class WebhookTransform : IWebhookTransform {
        private readonly JsonSerializerSettings _jsonSettings;
        private JsonSerializer _jsonSerializer;

        protected WebhookTransform(IJsonProvider jsonProvider) {
            _jsonSettings = jsonProvider.GetSettings();
        }
        
        public abstract object Apply(object body);
        public abstract bool IsTransform(object body);
        protected virtual void AddCustomConverters() { }

        protected void AddConverter<T>(Func<Type, bool> canConvert, Func<T, object> convert) {
            _jsonSettings.Converters.Insert(0, new CustomConverter<T>(canConvert, convert));
        }
        
        protected JsonSerializer GetJsonSerializer() {
            if (_jsonSerializer == null) {
                AddCustomConverters();
                
                _jsonSerializer = JsonSerializer.Create(_jsonSettings);
            }

            return _jsonSerializer;
        }
        
        protected class CustomConverter<T> : JsonConverter {
            private readonly Func<Type, bool> _canConvert;
            private readonly Func<T, object> _convert;

            public CustomConverter(Func<Type, bool> canConvert, Func<T, object> convert) {
                _canConvert = canConvert;
                _convert = convert;
            }
            
            public override bool CanConvert(Type objectType) {
                return _canConvert(objectType);
            }

            public override bool CanRead => false;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                var typedValue = (T) value;

                if (value == null) {
                    writer.WriteNull();
                } else {
                    serializer.Serialize(writer, _convert(typedValue));
                }
            }

            public override object ReadJson(JsonReader reader,
                                            Type objectType,
                                            object existingValue,
                                            JsonSerializer serializer) {
                throw new NotImplementedException();
            }
        }
    }
}