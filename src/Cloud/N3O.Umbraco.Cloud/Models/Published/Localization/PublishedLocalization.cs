using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLocalization : Value {
    public PublishedDateFormat DateFormat { get; set; }
    public PublishedNumberFormat NumberFormat { get; set; }
    public PublishedTimeFormat TimeFormat { get; set; }
    public PublishedTimezone Timezone { get; set; }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return DateFormat;
        yield return NumberFormat;
        yield return TimeFormat;
        yield return Timezone;
    }
}