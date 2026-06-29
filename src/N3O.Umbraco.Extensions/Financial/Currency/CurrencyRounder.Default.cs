using System;

namespace N3O.Umbraco.Financial;

public class DefaultCurrencyRounder : ICurrencyRounder {
    public virtual Money Round(Money money) => new Money(Math.Ceiling(money.Amount), money.Currency);
}
