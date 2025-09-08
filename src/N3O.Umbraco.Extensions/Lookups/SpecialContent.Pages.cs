using N3O.Umbraco.Content;

namespace N3O.Umbraco.Lookups;

public class SpecialPages : ISpecialContents {
    public static readonly SpecialContent Home = new("homePage", "Home Page", "homePage");
    public static readonly SpecialContent Donate = new("donatePage", "Donate Page", "donatePage");
    public static readonly SpecialContent NotFound = new("notFoundPage", "Not Found Page", "notFoundPage");
}