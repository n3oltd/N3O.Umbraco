using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutWebhookTransform : IWebhookTransform<Entities.Checkout> {
        public object Transform(IJsonProvider jsonProvider, Entities.Checkout entity) {
            var responses = entity.Account.Consent.Choices.ToList();

            var choices = new JObject();
            foreach (var choice in responses) {
                choices[choice.Channel.Id] = new JObject();
                choices[choice.Channel.Id][choice.Category.Id] = choice.Response.Id;
            }

            var json = jsonProvider.SerializeObject(entity);
            var jObject = JObject.Parse(json);

            jObject["account"]["consent"] = choices;
            return jObject;
        }
    }
}