using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLocalization {
    public DateFormat DateFormat { get; set; }
    public NumberFormat NumberFormat { get; set; }
    public TimeFormat TimeFormat { get; set; }
    public PublishedTimezone Timezone { get; set; }
    public Currency BaseCurrency { get; set; }
    public PublishedTerminology Terminology { get; set; }
}