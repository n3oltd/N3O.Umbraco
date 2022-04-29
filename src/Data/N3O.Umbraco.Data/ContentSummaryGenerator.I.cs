using N3O.Umbraco.Content;

namespace N3O.Umbraco.Data {
    public interface IContentSummaryGenerator {
        string GenerateSummary(ContentProperties contentProperties);
        bool IsGenerator(string contentTypeAlias);
    }
}