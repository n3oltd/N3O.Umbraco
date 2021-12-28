using N3O.Umbraco.Financial;
using NodaTime;
using Refit;

namespace N3O.Umbraco.Forex.Currencylayer;

public class HistoricalRateRequest : ApiRequest {
    private readonly LocalDate _date;

    public HistoricalRateRequest(Currency baseCurrency, Currency quoteCurrency, LocalDate date)
        : base(baseCurrency, quoteCurrency) {
        _date = date;
    }

    [AliasAs("date")]
    public string DateYearMonthDay => $"{_date.Year}-{_date.Month:00}-{_date.Day:00}";
}
