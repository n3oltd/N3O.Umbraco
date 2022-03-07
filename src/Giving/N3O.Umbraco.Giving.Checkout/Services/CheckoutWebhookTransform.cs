using N3O.Umbraco.Giving.Webhooks;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutWebhookTransform : IWebhookTransform {
        private readonly IJsonProvider _jsonProvider;

        public CheckoutWebhookTransform(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }

        public bool IsTransform(object body) => body is Entities.Checkout; 
        
        public object Apply(object body) {
            var checkout = (Entities.Checkout) body;
            
            var jObject = JObject.FromObject(checkout, JsonSerializer.Create(_jsonProvider.GetSettings()));

            var choices = checkout.Account.Consent.Choices.ToList();
            var channels = choices.Select(x => x.Channel).Distinct().ToList();

            foreach (var channel in channels) {
                jObject["account"]["consent"][channel.Id] = new JObject();
            }
            
            foreach (var choice in choices) {
                jObject["account"]["consent"][choice.Channel.Id][choice.Category.Id] = choice.Response.Value;
            }

            jObject["account"]["Address"]["country"] = checkout.Account.Address.Country.Iso2Code;
            
            return jObject;
        }
    }
}