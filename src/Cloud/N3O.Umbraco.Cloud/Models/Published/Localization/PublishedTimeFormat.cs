using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedTimeFormat {
    public string Id { get; set; }
    public string CultureCode { get; set; }
    public bool HasMeridiem { get; set; }
    
    public TimeFormat ToTimeFormat() {
        return new TimeFormat(Id, Id, HasMeridiem, CultureCode);
    }
}
