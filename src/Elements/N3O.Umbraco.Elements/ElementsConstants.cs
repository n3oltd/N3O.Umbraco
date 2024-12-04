namespace N3O.Umbraco.Elements;

public static class ElementsConstants {
    public static class DonationCategories {
        public static readonly string Alias = "donationCategories";
    }
    
    public static class DonationCategory {
        public static readonly string Alias = "donationCategory";
    }
    
    public static class EphemeralDonationCategory {
        public static readonly string Alias = "ephemeralDonationCategory";

        public static class Properties {
            public static readonly string EndOn = "endOn";
            public static readonly string StartOn = "startOn";
        }
    }
    
    public static class GeneralDonationCategory {
        public static readonly string Alias = "generalDonationCategory";
    }
    
    public static class DonationOption {
        public static readonly string Alias = "donationOption";
    }
}