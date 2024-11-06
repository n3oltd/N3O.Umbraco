namespace N3O.Umbraco.Payments.PayPal;

public static class PayPalConstants {
    public const string ApiName = "PayPal";
    public const string PlanStatus = "ACTIVE";
    public const string TenureType = "TENURE";
    public const string FrequencyInterval = "MONTH";
    public const int FrequencyCount = 1;
    public const int BillingCycleSequence = 1;
    public const int BillingCycleTotalCycles = 1;
    public const string ProductId = "N3O-Website-Donations";
    public const string ProductType = "SERVICE";
    public const string ProductCategory = "SOFTWARE";
    public const int PaymentFailureThreshold = 3;
    public const string SetupFeeFailureAction = "CONTINUE";
    public static readonly PayPalPaymentMethod PaymentMethod = new();
}
