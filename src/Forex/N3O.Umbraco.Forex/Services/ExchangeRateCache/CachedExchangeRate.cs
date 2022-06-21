using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using NodaTime;

namespace N3O.Umbraco.Forex;

public class CachedExchangeRate : Entity {
    public LocalDate Date { get; private set; }
    public Currency BaseCurrency { get; private set; }
    public Currency QuoteCurrency { get; private set; }
    public decimal Rate { get; private set; }

    public static CachedExchangeRate Create(EntityId id,
                                            LocalDate date,
                                            Currency baseCurrency,
                                            Currency quoteCurrency,
                                            decimal rate) {
        var cachedExchangeRate = Entity.Create<CachedExchangeRate>(id);
        cachedExchangeRate.Date = date;
        cachedExchangeRate.BaseCurrency = baseCurrency;
        cachedExchangeRate.QuoteCurrency = quoteCurrency;
        cachedExchangeRate.Rate = rate;

        return cachedExchangeRate;
    }
}
