namespace N3O.Umbraco.Payments.GoCardless;

public static class GoCardlessConstants {
    public const string ApiName = "GoCardless";
    public static readonly GoCardlessPaymentMethod PaymentMethod = new();
    
    public static class Codes {
        public static class Currencies {
            public static readonly string GBP = "GBP";
        }

        public static class Iso3CountryCodes {
            public static readonly string UnitedKingdom = "GBR";
        }
    }
}
