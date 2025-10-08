namespace N3O.Umbraco.Giving.Checkout;

public static class CheckoutConstants {
    public const string ApiName = "Checkout";
    
    public static class BlockModuleKeys {
        public static readonly string CheckoutAccount = nameof(CheckoutAccount);
        public static readonly string CheckoutComplete = nameof(CheckoutComplete);
        public static readonly string CheckoutDonation = nameof(CheckoutDonation);
        public static readonly string CheckoutError = nameof(CheckoutError);
        public static readonly string CheckoutRegularGiving = nameof(CheckoutRegularGiving);
    }

    public static class Environment {
        public static class Keys {
            public static string SiteLanguageTag = "SiteLanguageTag";
            public static string SiteNameTag = "SiteNameTag";
        }
    }
}
