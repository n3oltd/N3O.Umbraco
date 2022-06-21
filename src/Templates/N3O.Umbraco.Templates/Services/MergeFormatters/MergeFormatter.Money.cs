using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates;

public class MoneyFormatter : MergeFormatter<Money> {
    public MoneyFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(Money value) {
        return Formatter.Number.FormatMoney(value);
    }
}
