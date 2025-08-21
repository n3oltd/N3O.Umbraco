namespace N3O.Umbraco.Giving.Allocations;

public class AllocationsConstants {
    public static class Aliases {
        public static class DonationItem {
            public const string ContentType = "donationItem";

            public static class Properties {
                public const string AllowedGivingTypes = "allowedGivingTypes";
                public const string Dimension1 = "dimension1Options";
                public const string Dimension2 = "dimension2Options";
                public const string Dimension3 = "dimension3Options";
                public const string Dimension4 = "dimension4Options";
                public const string PricingRules = "priceRules";
            }
        }

        public static class DonationCampaign {
            public const string ContentType = "donationCampaign";
        }
        
        public static class DonationOption {
            public const string ContentType = "donationOption";
            
            public static class Properties {
                public const string Dimension1 = "dimension1";
                public const string Dimension2 = "dimension2";
                public const string Dimension3 = "dimension3";
                public const string Dimension4 = "dimension4";
            }
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
        
        public static class FeedbackScheme {
            public const string ContentType = "scheme";
            
            public static class Properties {
                public const string Dimension1 = "dimension1Options";
                public const string Dimension2 = "dimension2Options";
                public const string Dimension3 = "dimension3Options";
                public const string Dimension4 = "dimension4Options";
                public const string PricingRules = "priceRules";
            }
        }
        
        public static class Price {
            public static class Properties {
                public const string Amount = "priceAmount";
                public const string Locked = "priceLocked";
            }
        }
        
        public static class PriceHandle {
            public const string ContentType = "priceHandle";

            public static class Properties {
                public const string Amount = "amount";
                public const string Description = "description";
            }
        }
        
        public static class PricingRule {
            public const string ContentType = "pricingRule";

            public static class Properties {
                public const string Amount = "priceAmount";
                public const string Dimension1 = "dimension1";
                public const string Dimension2 = "dimension2";
                public const string Dimension3 = "dimension3";
                public const string Dimension4 = "dimension4";
                public const string Locked = "priceLocked";
            }
        }
        
        public static class SponsorshipScheme {
            public static class Properties {
                public const string Dimension1 = "dimension1Options";
                public const string Dimension2 = "dimension2Options";
                public const string Dimension3 = "dimension3Options";
                public const string Dimension4 = "dimension4Options";
                public const string PricingRules = "priceRules";
            }
        }
        
        public static class UpsellOffer {
            public static class Properties {
                public const string DonationItem = "donationItem";
                public const string Dimension1 = "dimension1";
                public const string Dimension2 = "dimension2";
                public const string Dimension3 = "dimension3";
                public const string Dimension4 = "dimension4";
            }
        }
    }
}