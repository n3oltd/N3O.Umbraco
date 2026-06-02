using System.Threading.Tasks;

namespace N3O.Umbraco.Blocks.Perplex;

public interface IPerplexBlockTypesService {
    Task CreateTypesAsync(PerplexBlockDefinition definition);
}
