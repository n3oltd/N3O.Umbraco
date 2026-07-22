namespace N3O.Umbraco.Financial;

public interface ICurrencyRounder {
    Money Round(Money money);
}
