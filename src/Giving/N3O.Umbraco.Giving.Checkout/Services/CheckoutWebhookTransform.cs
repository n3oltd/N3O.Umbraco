using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Webhooks;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutWebhookTransform : IWebhookTransform {
        private readonly JsonSerializerSettings _jsonSettings;

        public CheckoutWebhookTransform(IJsonProvider jsonProvider) {
            _jsonSettings = jsonProvider.GetSettings();
        }

        public bool IsTransform(object body) => body is Entities.Checkout; 
        
        public object Apply(object body) {
            var checkout = (Entities.Checkout) body;
            var serializer = GetSerializer();
            var jObject = JObject.FromObject(checkout, serializer);

            var choices = checkout.Account.Consent.Choices.ToList();
            var channels = choices.Select(x => x.Channel).Distinct().ToList();

            var consent = (JObject) jObject["account"]["consent"];
            
            foreach (var channel in channels) {
                consent[channel.Id] = new JObject();
            }
            
            foreach (var choice in choices) {
                consent[choice.Channel.Id][choice.Category.Id] = choice.Response.Value;
            }

            return jObject;
        }

        private JsonSerializer GetSerializer() {
            AddConverter<INamedLookup>(t => t.ImplementsInterface<INamedLookup>(),
                                       x => new { x.Id, x.Name });
            
            AddConverter<Country>(t => t == typeof(Country),
                                       x => new { x.Id, x.Name, x.Iso2Code, x.Iso3Code });

            return JsonSerializer.Create(_jsonSettings);
        }

        private void AddConverter<T>(Func<Type, bool> canConvert, Func<T, object> convert) {
            _jsonSettings.Converters.Insert(0, new CustomConverter<T>(canConvert, convert));
        }

        private class CustomConverter<T> : JsonConverter {
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