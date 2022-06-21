using N3O.Umbraco.Data.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Parsing;

public interface IBlobResolver {
    bool CanResolve(string value);
    Task<ParseResult<Blob>> ResolveAsync(string value);
}
