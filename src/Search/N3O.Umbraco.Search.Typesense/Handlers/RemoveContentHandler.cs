using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class RemoveContentHandler : IRequestHandler<RemoveContentCommand, None, None> {
    private readonly IEnumerable<ISearchIndexer> _searchIndexers;

    public RemoveContentHandler(IEnumerable<ISearchIndexer> searchIndexers) {
        _searchIndexers = searchIndexers;
    }

    public async Task<None> Handle(RemoveContentCommand req, CancellationToken cancellationToken) {
        var searchIndexers = _searchIndexers.Where(x => x.CanIndex(req.ContentType.Value)).OrEmpty().ToList();

        if (searchIndexers.HasAny()) {
            foreach (var searchIndexer in searchIndexers) {
                await searchIndexer.DeleteAsync(req.ContentId.Value);
            }
        }

        return None.Empty;
    }
}
