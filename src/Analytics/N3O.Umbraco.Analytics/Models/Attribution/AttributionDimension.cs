using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionDimension : Value {
    public AttributionDimension(int index, IEnumerable<OptionPercentage> optionPercentages) {
        Index = index;
        OptionPercentages = optionPercentages;
    }

    public int Index { get; }
    public IEnumerable<OptionPercentage> OptionPercentages { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Index;
        yield return OptionPercentages;
    }
}