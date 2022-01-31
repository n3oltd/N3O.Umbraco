namespace N3O.Umbraco.Payments.Content {
    public interface IPaymentMethodSettings {
        string TransactionDescription { get; }
        string TransactionId { get; }
    }
}