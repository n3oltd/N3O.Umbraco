namespace N3O.Umbraco.Forex {
    public interface IForexConverter {
        BaseToQuoteForexConverter BaseToQuote();
        QuoteToBaseForexConverter QuoteToBase();
    }
}
