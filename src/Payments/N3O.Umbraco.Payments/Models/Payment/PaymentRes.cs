namespace N3O.Umbraco.Payments.Models;

public class PaymentRes : PaymentObjectRes {
    public CardPaymentRes Card { get; set; }
    public string DeclinedReason { get; set; }
    public bool IsDeclined { get; set; }
    public bool IsPaid { get; set; }
}
