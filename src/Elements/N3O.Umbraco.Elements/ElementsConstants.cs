﻿namespace N3O.Umbraco.Elements;

public static class ElementsConstants {
    public static class Cdn {
        public static class Paths {
            public static readonly string FlowPaymentMethods = "payment-methods.json";
        }
    }
    
    public static class DonationCategory {
        public const string CompositionAlias = "donationCategory";
        public const string CropAlias = "m";

        public static class Dimension {
            public const string Alias = "dimensionDonationCategory";
            
            public static class Properties {
                public const string FundDimension = "dimension";
            }
        }
        
        public static class Ephemeral {
            public const string Alias = "ephemeralDonationCategory";

            public static class Properties {
                public const string EndOn = "endOn";
                public const string StartOn = "startOn";
            }
        }
        
        public static class General {
            public const string Alias = "generalDonationCategory";
        }
    }
    
    public static class DonationOption {
        public const string CompositionAlias = "donationOption";
        public const string CropAlias = "m";

        public static class Feedback {
            public const string Alias = "feedbackDonationOption";
        }
        
        public static class Fund {
            public const string Alias = "fundDonationOption";
        }
        
        public static class Sponsorship {
            public const string Alias = "sponsorshipDonationOption";
        }
    }

    public static class ElementsSettings {
        public const string Alias = "elementsSettings";
    }
    
    public static class ElementsCheckoutCompleteSettings {
        public const string Alias = "elementsCheckoutCompleteSettings";
    }

    public static class Giving {
        public const string Alias = "giving";
    }
}