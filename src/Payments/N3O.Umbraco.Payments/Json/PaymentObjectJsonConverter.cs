using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace N3O.Umbraco.Payments.Json {
    public class PaymentObjectJsonConverter : JsonConverter {
        private readonly ILookups _lookups;

        public PaymentObjectJsonConverter(ILookups lookups) {
            _lookups = lookups;
        }

        public override bool CanConvert(Type objectType) {
            return objectType.IsAssignableTo(typeof(PaymentObject));
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null || reader.Value == null) {
                return null;
            }

            if (reader.TokenType != JsonToken.StartObject) {
                throw new JsonSerializationException();
            }

            var jObject = JObject.Load(reader);
            var properties = jObject.Properties().ToList();

            var paymentMethodProperty = properties.Single(p => p.Name.EqualsInvariant(nameof(PaymentObject.Method)));
            var paymentMethodId = paymentMethodProperty.Value<string>();
            var paymentMethod = _lookups.FindById<PaymentMethod>(paymentMethodId);
            
            var paymentObjectTypeProperty = properties.Single(p => p.Name.EqualsInvariant(nameof(PaymentObject.Type)));
            var paymentObjectTypeId = paymentObjectTypeProperty.Value<string>();
            var paymentObjectType = _lookups.FindById<PaymentObjectType>(paymentObjectTypeId);
            
            var paymentObject = jObject.ToObject(paymentMethod.GetType(paymentObjectType));

            return paymentObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}