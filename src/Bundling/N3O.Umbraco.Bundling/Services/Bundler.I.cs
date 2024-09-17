using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Bundling;

public interface IBundler {
    Task<IEnumerable<string>> GenerateCssUrlsAsync();
    Task<IEnumerable<string>> GenerateJsUrlsAsync();
    IBundler RequiresCss(params string[] paths);
    IBundler RequiresJs(params string[] paths);
}