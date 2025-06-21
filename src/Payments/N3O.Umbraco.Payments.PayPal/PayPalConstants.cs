namespace N3O.Umbraco.Payments.PayPal;

public static class PayPalConstants {
    public const string ApiName = "PayPal";
    public static readonly int BillingCycleSequence = 1;
    public static readonly int BillingCycleTotalCycles = 0;
    public static readonly int FrequencyCount = 1;
    public static readonly string FrequencyInterval = "MONTH";
    public static readonly int PaymentFailureThreshold = 3;
    public static readonly string PlanStatus = "ACTIVE";
    public static readonly string ProductCategory = "SOFTWARE";
    public static readonly string ProductId = "N3O-Website-Donations";
    public static readonly string ProductType = "SERVICE";
    public static readonly string SetupFeeFailureAction = "CONTINUE";
    public static readonly string TenureType = "REGULAR";
    
    public static readonly PayPalPaymentMethod PaymentMethod = new();
}
