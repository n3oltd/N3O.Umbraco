using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using Slugify;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class SuggestSlugHandler : IRequestHandler<SuggestSlugQuery, string, string> {
    private readonly ISlugHelper _slugHelper;

    public SuggestSlugHandler(ISlugHelper slugHelper) {
        _slugHelper = slugHelper;
    }
    
    public Task<string> Handle(SuggestSlugQuery req, CancellationToken cancellationToken) {
        var slug = req.Model.IfNotNull(_slugHelper.GenerateSlug);

        return Task.FromResult(slug);
    }
}