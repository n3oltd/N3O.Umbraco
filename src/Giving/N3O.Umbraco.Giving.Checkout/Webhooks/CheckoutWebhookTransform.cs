using Humanizer;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Transforms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Webhooks {
    public class CheckoutWebhookTransform : WebhookTransform {
        public CheckoutWebhookTransform(IJsonProvider jsonProvider) : base(jsonProvider) { }

        public override bool IsTransform(object body) => body is Entities.Checkout;

        public override object Apply(object body) {
            var checkout = (Entities.Checkout) body;
            var serializer = GetJsonSerializer();
            var jObject = JObject.FromObject(checkout, serializer);

            TransformConsent(checkout, jObject);
            TransformSponsorships(serializer, GivingTypes.Donation, checkout.Donation?.Allocations, jObject);
            TransformSponsorships(serializer, GivingTypes.RegularGiving, checkout.RegularGiving?.Allocations, jObject);

            return jObject;
        }

        private void TransformConsent(Entities.Checkout checkout, JObject jObject) {
            var choices = checkout.Account.Consent.Choices.ToList();
            var channels = choices.Select(x => x.Channel).Distinct().ToList();

            var consent = (JObject) jObject["account"]["consent"];

            foreach (var channel in channels) {
                consent[channel.Id] = new JObject();
            }

            foreach (var choice in choices) {
                consent[choice.Channel.Id][choice.Category.Id] = choice.Response.Value;
            }
        }

        private void TransformSponsorships(JsonSerializer serializer,
                                           GivingType givingType,
                                           IEnumerable<Allocation> allocations,
                                           JObject jObject) {
            foreach (var allocation in allocations.OrEmpty().Where(x => x.Type == AllocationTypes.Sponsorship)) {
                var key = $"{givingType.Id}{allocation.Sponsorship.Scheme.Id.Pascalize()}Sponsorships";

                if (!jObject.ContainsKey(key)) {
                    jObject[key] = new JArray();
                }

                var allocationJObject = JObject.FromObject(allocation, serializer);

                if (allocation.Sponsorship.Duration != null) {
                    allocationJObject["value"]["amount"] = allocation.Value.Amount / allocation.Sponsorship.Duration.Months;
                    
                    var components = (JArray) allocationJObject["sponsorship"]["components"];
                    
                    foreach (var component in components) {
                        var amount = component["value"]["amount"].ToObject<decimal>();
                        
                        component["value"]["amount"] = amount / allocation.Sponsorship.Duration.Months;
                    }
                }

                ((JArray) jObject[key]).Add(allocationJObject);
            }
        }

        protected override void AddCustomConverters() {
            AddConverter<INamedLookup>(t => t.ImplementsInterface<INamedLookup>(),
                                       x => new {x.Id, x.Name});

            AddConverter<Country>(t => t == typeof(Country),
                                  x => new {x.Id, x.Name, x.Iso2Code, x.Iso3Code});
        }
    }
}