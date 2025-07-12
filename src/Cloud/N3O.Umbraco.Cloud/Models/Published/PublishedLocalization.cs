using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLocalization : Value {
    public DateFormat DateFormat { get; set; }
    public NumberFormat NumberFormat { get; set; }
    public TimeFormat TimeFormat { get; set; }
    public PublishedTimezone Timezone { get; set; }
    public Currency BaseCurrency { get; set; }
    public PublishedTerminology Terminology { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DateFormat;
        yield return NumberFormat;
        yield return TimeFormat;
        yield return Timezone;
        yield return BaseCurrency;
        yield return Terminology;
    }
}