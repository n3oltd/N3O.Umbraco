using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Financial;

// Default rounds converted amounts up to a whole unit; downstream sites can register their own ICurrencyRounder
public class DefaultCurrencyRounder : ICurrencyRounder {
    public virtual Money Round(Money money) => money.RoundUpToWholeNumber();
}
