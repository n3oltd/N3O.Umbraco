using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class OptionPercentage : Value {
    public OptionPercentage(string option, decimal percentage) {
        Option = option ?? "[Empty]";
        Percentage = percentage;
    }

    public string Option { get; }
    public decimal Percentage { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Option;
        yield return Percentage;
    }
}