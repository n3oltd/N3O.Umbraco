using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPreviewHtmlGenerator {
    bool CanGeneratePreview(string contentTypeAlias);
    Task<(string ETag, string Html)> GeneratePreviewHtmlAsync(IReadOnlyDictionary<string, object> content);
}