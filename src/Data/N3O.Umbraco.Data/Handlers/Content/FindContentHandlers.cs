using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.QueryFilters;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Handlers;

public class FindContentHandlers :
    IRequestHandler<GetContentByIdQuery, None, ContentRes>,
    IRequestHandler<FindChildrenQuery, ContentCriteria, IEnumerable<ContentRes>>,
    IRequestHandler<FindDescendantsQuery, ContentCriteria, IEnumerable<ContentRes>> {
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly ContentQueryFilter _contentQueryFilter;

    public FindContentHandlers(IContentLocator contentLocator,
                               IJsonProvider jsonProvider,
                               ContentQueryFilter contentQueryFilter) {
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _contentQueryFilter = contentQueryFilter;
    }
    
    public Task<ContentRes> Handle(GetContentByIdQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById, true);

        return Task.FromResult(ToContentRes(content));
    }

    public Task<IEnumerable<ContentRes>> Handle(FindChildrenQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById, true);

        return Task.FromResult(FilterAndConvert(content.Children, req.Model));
    }

    public Task<IEnumerable<ContentRes>> Handle(FindDescendantsQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById, true);

        return Task.FromResult(FilterAndConvert(content.Descendants(), req.Model));
    }

    private IEnumerable<ContentRes> FilterAndConvert(IEnumerable<IPublishedContent> contents,
                                                     ContentCriteria criteria) {
        contents = _contentQueryFilter.Apply(contents, criteria);

        return contents.Select(ToContentRes).ToList();
    }

    private ContentRes ToContentRes(IPublishedContent content) {
        return _jsonProvider.DeserializeObject<ContentRes>(_jsonProvider.SerializeObject(content));
    }
}
