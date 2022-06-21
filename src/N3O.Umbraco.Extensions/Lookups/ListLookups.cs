using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class ListLookups<TLookup> where TLookup : INamedLookup {
    private readonly ILookups _lookups;
    private readonly IUmbracoMapper _mapper;

    public ListLookups(ILookups lookups, IUmbracoMapper mapper) {
        _lookups = lookups;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NamedLookupRes>> RunAsync(CancellationToken cancellationToken = default) {
        var lookups = await _lookups.GetAllAsync<TLookup>(cancellationToken);
        var mappedLookups = lookups.Select(x => _mapper.Map<INamedLookup, NamedLookupRes>(x))
                                   .OrderBy(x => x.Name)
                                   .ToList();

        return mappedLookups;
    }
}
