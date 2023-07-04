namespace N3O.Umbraco.Payments.DirectDebitUK;

public static class DirectDebitUKConstants {
    public const string ApiName = "DirectDebitUK";
    public static readonly DirectDebitUKPaymentMethod PaymentMethod = new();
    
    public static class Codes {
        public static class Currencies {
            public const string GBP = "GBP";
        }

        public static class Iso3CountryCodes {
            public const string UnitedKingdom = "GBR";
        }
    }
}