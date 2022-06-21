namespace N3O.Umbraco.Giving.Checkout;

public static class CheckoutConstants {
    public const string ApiName = "Checkout";
    
    public static class BlockModuleKeys {
        public const string CheckoutAccount = nameof(CheckoutAccount);
        public const string CheckoutComplete = nameof(CheckoutComplete);
        public const string CheckoutDonation = nameof(CheckoutDonation);
        public const string CheckoutError = nameof(CheckoutError);
        public const string CheckoutRegularGiving = nameof(CheckoutRegularGiving);
    }
}
