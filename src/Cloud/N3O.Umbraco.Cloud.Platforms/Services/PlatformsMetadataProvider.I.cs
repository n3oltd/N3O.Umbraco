using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPlatformsMetadataProvider {
    Task<bool> IsProviderForAsync();
    Task<IReadOnlyDictionary<string, object>> GetAsync();
}