using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedNumberFormat {
    public string Id { get; set; }
    public string CultureCode { get; set; }
    public string DecimalSeparator { get; set; }
    public string ThousandsSeparator { get; set; }

    public NumberFormat ToNumberFormat() {
        return new NumberFormat(Id, Id, CultureCode, DecimalSeparator, ThousandsSeparator);
    }
}
