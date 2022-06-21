namespace N3O.Umbraco.Financial;

public class MoneyRes {
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public string Text { get; set; }

    public static implicit operator Money(MoneyRes res) {
        return new Money(res.Amount, res.Currency);
    }
}
