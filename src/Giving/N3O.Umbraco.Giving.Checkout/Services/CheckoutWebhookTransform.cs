using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutWebhookTransform : IWebhookTransform<Entities.Checkout> {
        public object Transform(IJsonProvider jsonProvider, Entities.Checkout entity) {
            var choices = entity.Account.Consent.Choices.ToList();

            var responses = new JObject();
            foreach (var choice in choices) {
                responses[choice.Channel.Id] = new JObject();
                responses[choice.Channel.Id][choice.Category.Id] = choice.Response.Id;
            }

            var json = jsonProvider.SerializeObject(entity);
            var jObject = JObject.Parse(json);

            jObject["account"]["consent"] = responses;
            return jObject;
        }
    }
}