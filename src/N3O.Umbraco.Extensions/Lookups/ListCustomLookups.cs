using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class ListCustomLookups<TLookup, TRes> where TLookup : ILookup {
    private readonly ILookups _lookups;
    private readonly IUmbracoMapper _mapper;

    public ListCustomLookups(ILookups lookups, IUmbracoMapper mapper) {
        _lookups = lookups;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TRes>> RunAsync(CancellationToken cancellationToken = default) {
        var lookups = await _lookups.GetAllAsync<TLookup>(cancellationToken);
        var res = lookups.Select(_mapper.Map<TLookup, TRes>).ToList();

        return res;
    }
}