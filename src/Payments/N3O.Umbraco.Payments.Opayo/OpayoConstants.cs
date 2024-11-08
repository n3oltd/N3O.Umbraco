namespace N3O.Umbraco.Payments.Opayo;

public static class OpayoConstants {
    public const string ApiName = "Opayo";
    public static readonly OpayoPaymentMethod PaymentMethod = new();
    
    public static class Iso3CountryCodes {
        public static readonly string UnitedStates = "USA";
    }
}
