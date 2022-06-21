namespace N3O.Umbraco.Payments.GoCardless;

public static class GoCardlessConstants {
    public const string ApiName = "GoCardless";
    public static readonly GoCardlessPaymentMethod PaymentMethod = new();
    
    public static class Codes {
        public static class Currencies {
            public const string GBP = "GBP";
        }

        public static class Iso3CountryCodes {
            public const string UnitedKingdom = "GBR";
        }
    }
}
