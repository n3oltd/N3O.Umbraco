namespace N3O.Umbraco.Financial;

// Register downstream to turn rounding off; amounts pass through unchanged
public class NullCurrencyRounder : ICurrencyRounder {
    public virtual Money Round(Money money) => money;
}
