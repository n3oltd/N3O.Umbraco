namespace N3O.Umbraco.Financial;

public class NullCurrencyRounder : ICurrencyRounder {
    public virtual Money Round(Money money) => money;
}
