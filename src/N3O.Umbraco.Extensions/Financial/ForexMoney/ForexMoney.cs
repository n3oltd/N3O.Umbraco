namespace N3O.Umbraco.Financial;

public partial class ForexMoney {
    public ForexMoney(Money @base, Money quote, decimal exchangeRate) {
        Base = @base;
        Quote = quote;
        ExchangeRate = exchangeRate;
    }

    public ForexMoney(Money @base, Money quote) {
        Base = @base;
        Quote = quote;

        if (quote.Amount == 0 && @base.Amount == 0) {
            ExchangeRate = 1;
        } else {
            ExchangeRate = quote.Amount / @base.Amount;
        }
    }

    public Money Base { get; }
    public Money Quote { get; }
    public decimal ExchangeRate { get; }
}