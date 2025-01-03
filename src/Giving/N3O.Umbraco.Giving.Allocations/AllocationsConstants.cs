﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Content;

namespace N3O.Umbraco.Giving.Allocations;

public class AllocationsConstants {
    public static class Aliases {
        public static class DonationItem {
            public static readonly string ContentType = AliasHelper<Allocations.Lookups.DonationItem>.ContentTypeAlias();

            public static class Properties {
                public static readonly string AllowedGivingTypes = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.AllowedGivingTypes);
                public static readonly string Dimension1Options = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.Dimension1Options);
                public static readonly string Dimension2Options = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.Dimension2Options);
                public static readonly string Dimension3Options = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.Dimension3Options);
                public static readonly string Dimension4Options = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.Dimension4Options);
                public static readonly string PriceRules = AliasHelper<Allocations.Lookups.DonationItem>.PropertyAlias(x => x.PriceRules);
            }
        }

        public static class DonationCampaign {
            public static readonly string ContentType = "donationCampaign";
        }
        
        public static class DonationOption {
            public static readonly string ContentType = "donationOption";
        }
        
        public static class FeedbackCustomField {
            public static readonly string ContentType = "feedbackCustomField";

            public static class Properties {
                public static readonly string Alias = "alias";
                public static readonly string Bool = "bool";
                public static readonly string Date = "date";
                public static readonly string Name = "displayName";
                public static readonly string Text = "text";
                public static readonly string Type = "type";
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
}