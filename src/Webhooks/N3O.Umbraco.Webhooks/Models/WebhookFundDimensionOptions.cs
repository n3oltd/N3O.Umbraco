using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookFundDimensionOptions : Value {
    public WebhookFundDimensionOptions(IEnumerable<string> dimension1,
                                       IEnumerable<string> dimension2,
                                       IEnumerable<string> dimension3,
                                       IEnumerable<string> dimension4) {
        Dimension1 = dimension1;
        Dimension2 = dimension2;
        Dimension3 = dimension3;
        Dimension4 = dimension4;
    }

    public IEnumerable<string> Dimension1 { get; }
    public IEnumerable<string> Dimension2 { get; }
    public IEnumerable<string> Dimension3 { get; }
    public IEnumerable<string> Dimension4 { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Dimension1;
        yield return Dimension2;
        yield return Dimension3;
        yield return Dimension4;
    }
}