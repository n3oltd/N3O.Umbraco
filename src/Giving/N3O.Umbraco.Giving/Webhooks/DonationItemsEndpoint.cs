using N3O.Giving.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Endpoints;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aliases = N3O.Umbraco.Giving.GivingConstants.Aliases;

namespace N3O.Umbraco.Giving.Webhooks {
    [WebhookEndpoint(GivingConstants.Webhooks.Endpoints.DonationItems)]
    public class DonationItemsEndpoint : WebhookEndpoint<DonationItemsEndpoint.DonationItem> {
        private readonly IContentCache _contentCache;
        private readonly IContentEditor _contentEditor;
        private readonly ILookups _lookups;

        public DonationItemsEndpoint(IJsonProvider jsonProvider,
                                     IContentCache contentCache,
                                     IContentEditor contentEditor,
                                     ILookups lookups)
            : base(jsonProvider) {
            _contentCache = contentCache;
            _contentEditor = contentEditor;
            _lookups = lookups;
        }

        protected override Task HandleAsync(DonationItem donationItem, CancellationToken cancellationToken) {
            var allowedGivingTypes = GetLookups<GivingType>(donationItem.AllowedGivingTypes);
            var dimension1Options = GetLookups<FundDimension1Value>(donationItem.Dimension1Options);
            var dimension2Options = GetLookups<FundDimension2Value>(donationItem.Dimension2Options);
            var dimension3Options = GetLookups<FundDimension3Value>(donationItem.Dimension3Options);
            var dimension4Options = GetLookups<FundDimension4Value>(donationItem.Dimension4Options);
            
            var contentPublisher = GetContentPublisher(donationItem.Name);

            contentPublisher.Content.DataList(Aliases.DonationItem.Properties.AllowedGivingTypes).SetLookups(allowedGivingTypes);
            contentPublisher.Content.ContentPicker(Aliases.DonationItem.Properties.Dimension1Options).SetContent(dimension1Options);
            contentPublisher.Content.ContentPicker(Aliases.DonationItem.Properties.Dimension2Options).SetContent(dimension2Options);
            contentPublisher.Content.ContentPicker(Aliases.DonationItem.Properties.Dimension3Options).SetContent(dimension3Options);
            contentPublisher.Content.ContentPicker(Aliases.DonationItem.Properties.Dimension4Options).SetContent(dimension4Options);
            contentPublisher.Content.Numeric(Aliases.Price.Properties.Amount).SetDecimal(donationItem.Price?.Amount);
            contentPublisher.Content.Toggle(Aliases.Price.Properties.Locked).Set(donationItem.Price?.Locked);

            if (donationItem.PriceRules.HasAny()) {
                var nestedContent = contentPublisher.Content.Nested(Aliases.DonationItem.Properties.PriceRules);
                
                foreach (var priceRule in donationItem.PriceRules) {
                    AddPriceRule(nestedContent.Add(Aliases.PricingRule.ContentType), priceRule);
                }
            } else {
                contentPublisher.Content.Null(Aliases.DonationItem.Properties.PriceRules);
            }

            contentPublisher.SaveAndPublish();
            
            return Task.CompletedTask;
        }

        private IContentPublisher GetContentPublisher(string name) {
            var collection = _contentCache.Single<DonationItemsContent>();
            var match = collection.GetDonationItems().SingleOrDefault(x => x.Name.EqualsInvariant(name));

            if (match != null) {
                var contentPublisher = _contentEditor.ForExisting(match.Content().Key);
                contentPublisher.Rename(name);

                return contentPublisher;
            } else {
                return _contentEditor.New(name, collection.Content().Key, Aliases.DonationItem.ContentType);
            }
        }
        
        private IReadOnlyList<T> GetLookups<T>(IEnumerable<string> ids) where T : ILookup {
            return ids.OrEmpty().Select(x => _lookups.FindById<T>(x)).ExceptNull().ToList();
        }

        private void AddPriceRule(IContentBuilder contentBuilder, PricingRule priceRule) {
            contentBuilder.Numeric(Aliases.Price.Properties.Amount).SetDecimal(priceRule.Price?.Amount);
            contentBuilder.Toggle(Aliases.Price.Properties.Locked).Set(priceRule.Price?.Locked);
            contentBuilder.ContentPicker(Aliases.PricingRule.Properties.Dimension1).SetContent(_lookups.FindById<FundDimension1Value>(priceRule.Dimension1));
            contentBuilder.ContentPicker(Aliases.PricingRule.Properties.Dimension2).SetContent(_lookups.FindById<FundDimension2Value>(priceRule.Dimension2));
            contentBuilder.ContentPicker(Aliases.PricingRule.Properties.Dimension3).SetContent(_lookups.FindById<FundDimension3Value>(priceRule.Dimension3));
            contentBuilder.ContentPicker(Aliases.PricingRule.Properties.Dimension4).SetContent(_lookups.FindById<FundDimension4Value>(priceRule.Dimension4));
        }
        
        public class DonationItem {
            public string Name { get; set; }
            public IEnumerable<string> AllowedGivingTypes { get; set; }
            public IEnumerable<string> Dimension1Options { get; set; }
            public IEnumerable<string> Dimension2Options { get; set; }
            public IEnumerable<string> Dimension3Options { get; set; }
            public IEnumerable<string> Dimension4Options { get; set; }
            public Price Price { get; set; }
            public IEnumerable<PricingRule> PriceRules { get; set; }
        }

        public class Price {
            public decimal? Amount { get; set; }
            public bool? Locked { get; set; }
        }

        public class PricingRule {
            public Price Price { get; set; }
            public string Dimension1 { get; set; }
            public string Dimension2 { get; set; }
            public string Dimension3 { get; set; }
            public string Dimension4 { get; set; }
        }
    }
}