using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetContentPropertyValueHandler :
    IRequestHandler<GetContentPropertyValueQuery, None, ContentPropertyValueRes> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public GetContentPropertyValueHandler(IUmbracoMapper mapper, IContentLocator contentLocator) {
        _mapper = mapper;
        _contentLocator = contentLocator;
    }

    public Task<ContentPropertyValueRes> Handle(GetContentPropertyValueQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById, true);
        var property = req.PropertyAlias.Run(alias => content.Properties
                                                             .SingleOrDefault(x => x.Alias.EqualsInvariant(alias)),
                                             true);

        var publishedContentProperty = new PublishedContentProperty(content.ContentType.Alias, property);
        
        var res = _mapper.Map<PublishedContentProperty, ContentPropertyValueRes>(publishedContentProperty);
        
        return Task.FromResult(res);
    }
}