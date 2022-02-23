using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Payments.Json {
    public class PaymentObjectJsonConverter : JsonConverter {
        private readonly ILookups _lookups;

        public PaymentObjectJsonConverter(ILookups lookups) {
            _lookups = lookups;
        }

        public override bool CanConvert(Type objectType) {
            return objectType.IsAbstract && objectType.IsAssignableTo(typeof(PaymentObject));
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            if (reader.TokenType != JsonToken.StartObject) {
                throw new JsonSerializationException();
            }

            var jObject = JObject.Load(reader);
            var properties = jObject.Properties().ToList();

            var paymentMethod = GetPropertyValue<PaymentMethod>(properties, nameof(PaymentObject.Method));
            var paymentObjectType = GetPropertyValue<PaymentObjectType>(properties, nameof(PaymentObject.Type));
            
            var paymentObject = jObject.ToObject(paymentMethod.GetObjectType(paymentObjectType), serializer);

            return paymentObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        private T GetPropertyValue<T>(IReadOnlyList<JProperty> properties, string propertyName) where T : ILookup {
            var property = properties.SingleOrDefault(x => x.Name.EqualsInvariant(propertyName));
           
            if (property == null) {
                throw new Exception($"No property found with name {propertyName}");
            }

            var lookupId = (string) property.Value;
            var lookup = _lookups.FindById<T>(lookupId);

            if (lookup == null) {
                throw new Exception($"Property {propertyName} contains invalid value {lookupId}");
            }
            
            return lookup;
        }
    }
}