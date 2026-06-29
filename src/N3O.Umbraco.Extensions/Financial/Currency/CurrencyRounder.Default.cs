using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Financial;

public class DefaultCurrencyRounder : ICurrencyRounder {
    public virtual Money Round(Money money) => money.RoundUpToWholeNumber();
}
