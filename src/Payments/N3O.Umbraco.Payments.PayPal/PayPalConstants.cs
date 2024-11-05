namespace N3O.Umbraco.Payments.PayPal;

public static class PayPalConstants {
    public const string ApiName = "PayPal";
    public const string ProductId = "N3O-Website-Donations";
    public const string ProductType = "SERVICE";
    public const string ProductCategory = "SOFTWARE";
    public static readonly PayPalPaymentMethod PaymentMethod = new();
}
