using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetPropertyValueHandler : IRequestHandler<GetPropertyValueCommand, None, PagePropertyValueRes> {
    private readonly IContentService _contentService;
    private readonly IUmbracoMapper _mapper;

    public GetPropertyValueHandler(IUmbracoMapper mapper, IContentService contentService) {
        _mapper = mapper;
        _contentService = contentService;
    }

    public Task<PagePropertyValueRes> Handle(GetPropertyValueCommand req, CancellationToken cancellationToken) {
        var page = req.PageId.Run(_contentService.GetById, true);

        var property = page.Properties.SingleOrDefault(x => x.Alias == req.PropertyAlias.Value);
        
        var res = _mapper.Map<IProperty, PagePropertyValueRes>(property);
        
        return Task.FromResult(res);
    }
}