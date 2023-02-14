using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookFundDimensionValues : Value {
    public WebhookFundDimensionValues(string dimension1, string dimension2, string dimension3, string dimension4) {
        Dimension1 = dimension1;
        Dimension2 = dimension2;
        Dimension3 = dimension3;
        Dimension4 = dimension4;
    }

    public string Dimension1 { get; }
    public string Dimension2 { get; }
    public string Dimension3 { get; }
    public string Dimension4 { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Dimension1;
        yield return Dimension2;
        yield return Dimension3;
        yield return Dimension4;
    }
}