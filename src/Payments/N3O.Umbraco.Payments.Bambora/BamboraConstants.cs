namespace N3O.Umbraco.Payments.Bambora {
    public static class BamboraConstants {
        public const string ApiName = "Bambora";
        public static readonly BamboraPaymentMethod PaymentMethod = new();
        
        public static class Iso3CountryCodes {
            public const string Canada = "CAN";
            public const string UnitedStates = "USA";
        }
    }
}