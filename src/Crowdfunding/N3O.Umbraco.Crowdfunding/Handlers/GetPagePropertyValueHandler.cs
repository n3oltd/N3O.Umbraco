using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetPagePropertyValueHandler : IRequestHandler<GetPagePropertyValueQuery, None, PagePropertyValueRes> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public GetPagePropertyValueHandler(IUmbracoMapper mapper, IContentLocator contentLocator) {
        _mapper = mapper;
        _contentLocator = contentLocator;
    }

    public Task<PagePropertyValueRes> Handle(GetPagePropertyValueQuery req, CancellationToken cancellationToken) {
        var page = req.PageId.Run(_contentLocator.ById, true);
        var property = req.PropertyAlias.Run(alias => page.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(alias)), true);
        
        var res = _mapper.Map<IPublishedProperty, PagePropertyValueRes>(property);
        
        return Task.FromResult(res);
    }
}