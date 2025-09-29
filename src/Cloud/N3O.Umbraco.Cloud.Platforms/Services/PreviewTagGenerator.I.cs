using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPreviewTagGenerator {
    bool CanGeneratePreview(string contentTypeAlias);
    Task<(string ETag, string Html)> GeneratePreviewTagAsync(IReadOnlyDictionary<string, object> content);
}