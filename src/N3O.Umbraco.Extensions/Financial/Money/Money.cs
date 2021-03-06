namespace N3O.Umbraco.Financial;

public partial class Money : Value {
    public Money(decimal amount, Currency currency) {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public Currency Currency { get; }
}
