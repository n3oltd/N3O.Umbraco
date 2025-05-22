using N3O.Umbraco.Search.Typesense.Models;
using NodaTime;
namespace N3O.Umbraco.Search.Typesense;

public interface ISearchDocumentBuilder {
    ISearchDocumentBuilder IsPrimaryFor(string[] keywords);
    ISearchDocumentBuilder WithContent(string content);
    ISearchDocumentBuilder WithDescription(string description);
    ISearchDocumentBuilder WithTitle(string title);
    ISearchDocumentBuilder WithUrl(string url);
    ISearchDocumentBuilder WithTimestamp(Instant timestamp);
    SearchDocument Build();
}
