using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedDateFormat {
    public string Id { get; set; }
    public string CultureCode { get; set; }
    public string Pattern { get; set; }
    public string Separator { get; set; }

    public DateFormat ToDateFormat() {
        return new DateFormat(Id, Id, CultureCode, Pattern, Separator);
    }
}
