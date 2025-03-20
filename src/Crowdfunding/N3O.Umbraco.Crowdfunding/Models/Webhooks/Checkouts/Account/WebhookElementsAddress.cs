using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsAddress : Value {
    public WebhookElementsAddress(string line1,
                                  string line2,
                                  string line3,
                                  string locality,
                                  string administrativeArea,
                                  string postalCode,
                                  WebhookLookup country) {
        Line1 = line1;
        Line2 = line2;
        Line3 = line3;
        Locality = locality;
        AdministrativeArea = administrativeArea;
        PostalCode = postalCode;
        Country = country;
    }

    public string Line1 { get; }
    public string Line2 { get; }
    public string Line3 { get; }
    public string Locality { get; }
    public string AdministrativeArea { get; }
    public string PostalCode { get; }
    public WebhookLookup Country { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Line1;
        yield return Line2;
        yield return Line3;
        yield return Locality;
        yield return AdministrativeArea;
        yield return PostalCode;
        yield return Country;
    }
}