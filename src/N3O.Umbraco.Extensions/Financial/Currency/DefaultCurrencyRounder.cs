namespace N3O.Umbraco.Financial;

public class DefaultCurrencyRounder : ICurrencyRounder {
    public virtual decimal Round(decimal value) => value;
}
