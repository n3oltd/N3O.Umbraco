using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.Giving;

public static class GivingConstants {
    public const string ApiName = "Giving";

    public static class Aliases {
        public static class DonationItem {
            public static readonly string ContentType = AliasHelper<Lookups.DonationItem>.ContentTypeAlias();

            public static class Properties {
                public static readonly string AllowedGivingTypes = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.AllowedGivingTypes);
                public static readonly string Dimension1Options = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.Dimension1Options);
                public static readonly string Dimension2Options = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.Dimension2Options);
                public static readonly string Dimension3Options = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.Dimension3Options);
                public static readonly string Dimension4Options = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.Dimension4Options);
                public static readonly string PriceRules = AliasHelper<Lookups.DonationItem>.PropertyAlias(x => x.PriceRules);
            }
        }

        public static class DonationCampaign {
            public static readonly string ContentType = "donationCampaign";
        }
        
        public static class DonationOption {
            public static readonly string ContentType = "donationOption";
        }
        
        public static class FeedbackCustomField {
            public const string ContentType = "feedbackCustomField";

            public static class Properties {
                public const string Alias = "alias";
                public const string Bool = "bool";
                public const string Date = "date";
                public const string Name = "displayName";
                public const string Text = "text";
                public const string Type = "type";
            }
        }
        
        public static class Price {
            public static class Properties {
                public static readonly string Amount = AliasHelper<PriceContent>.PropertyAlias(x => x.Amount);
                public static readonly string Locked = AliasHelper<PriceContent>.PropertyAlias(x => x.Locked);
            }
        }
        
        public static class PriceHandle {
            public static readonly string ContentType = "priceHandle";

            public static class Properties {
                public static readonly string Amount = "amount";
                public static readonly string Description = "description";
            }
        }
        
        public static class PricingRule {
            public static readonly string ContentType = AliasHelper<PricingRuleElement>.ContentTypeAlias();

            public static class Properties {
                public static readonly string Dimension1 = AliasHelper<PricingRuleElement>.PropertyAlias(x => x.Dimension1);
                public static readonly string Dimension2 = AliasHelper<PricingRuleElement>.PropertyAlias(x => x.Dimension2);
                public static readonly string Dimension3 = AliasHelper<PricingRuleElement>.PropertyAlias(x => x.Dimension3);
                public static readonly string Dimension4 = AliasHelper<PricingRuleElement>.PropertyAlias(x => x.Dimension4);
            }
        }
    }

    public static class Webhooks {
        public static class EventTypes {
            public const string Published = "donationItem.published";
            public const string Unpublished = "donationItem.unpublished";
        }
        
        public static class HookIds {
            public const string DonationItem = nameof(DonationItem);
        }

        public static class Headers {
            public const string PreviousName = "N3O-Donation-Item-Previous-Name";
        }
    }
}
