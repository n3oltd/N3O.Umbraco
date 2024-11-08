namespace N3O.Umbraco.Payments.PayPal;

public static class PayPalConstants {
    public const int FrequencyCount = 1;
    public const int BillingCycleSequence = 1;
    public const int BillingCycleTotalCycles = 0;
    public const int PaymentFailureThreshold = 3;
    public const string ApiName = "PayPal";
    public const string FrequencyInterval = "MONTH";
    public const string PayeePreferred = "IMMEDIATE_PAYMENT_REQUIRED";
    public const string PayerSelected = "PAYPAL";
    public const string PlanStatus = "ACTIVE";
    public const string ProductId = "N3O-Website-Donations";    
    public const string ProductCategory = "SOFTWARE";
    public const string ProductType = "SERVICE";
    public const string SetupFeeFailureAction = "CONTINUE";
    public const string TenureType = "REGULAR";
    public static readonly PayPalPaymentMethod PaymentMethod = new();
}
