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

            var choices = checkout.Account.Consent.Choices.ToList();
            var channels = choices.Select(x => x.Channel).Distinct().ToList();

            var consent = (JObject) jObject["account"]["consent"];
            
            foreach (var channel in channels) {
                consent[channel.Id] = new JObject();
            }
            
            foreach (var choice in choices) {
                consent[choice.Channel.Id][choice.Category.Id] = choice.Response.Value;
            }

            if (checkout.Donation.IsComplete) {
                var sponsorships = GetSponsorships(checkout.Donation.Allocations, serializer).ToList();

                foreach (var (scheme, allocations) in sponsorships) {
                    jObject["donation"][$"{scheme.Id}Sponorships"] = allocations;
                }
            }

            if (checkout.RegularGiving.IsComplete) {
                var sponsorships = GetSponsorships(checkout.RegularGiving.Allocations, serializer).ToList();

                foreach (var (scheme, allocations) in sponsorships) {
                    jObject["regularGiving"][$"{scheme.Id}Sponorships"] = allocations;
                }
            }

            return jObject;
        }

        private IEnumerable<(SponsorshipScheme Scheme, JArray Allocations)> GetSponsorships(IEnumerable<IAllocation> allocations, JsonSerializer serializer) {
            var groupdedByScheme = allocations.Where(x => x.Type == AllocationTypes.Sponsorship)
                                              .GroupBy(x => x.Sponsorship.Scheme)
                                              .ToList();

            var res = new List<(SponsorshipScheme, JArray)>();

            foreach (var schemeSponsorships in groupdedByScheme) {
                var sponosrshipAllocations = schemeSponsorships.ToList();
                var sponosrshipAllocationsJArray = JArray.FromObject(sponosrshipAllocations, serializer);
                var result = (schemeSponsorships.Key, sponosrshipAllocationsJArray);

                res.Add(result);
            }

            return res;
        }

        protected override void AddCustomConverters() {
            AddConverter<INamedLookup>(t => t.ImplementsInterface<INamedLookup>(),
                                       x => new {x.Id, x.Name});

            AddConverter<Country>(t => t == typeof(Country),
                                  x => new {x.Id, x.Name, x.Iso2Code, x.Iso3Code});
        }
    }
}