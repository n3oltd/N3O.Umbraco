using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdateContributionHandler : IRequestHandler<UpdateContributionCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IContributionRepository _contributionRepository;
    private readonly ILookups _lookups;
    
    public UpdateContributionHandler(IContentLocator contentLocator,
                                     IContributionRepository contributionRepository,
                                     ILookups lookups) {
        _contentLocator = contentLocator;
        _contributionRepository = contributionRepository;
        _lookups = lookups;
    }

    public async Task<None> Handle(UpdateContributionCommand req, CancellationToken cancellationToken) {
        var crowdfunderType = req.TypeId.Run(_lookups.FindById<CrowdfunderType>, true);
        var crowdfunderContent = req.ContentId.Value.ToCrowdfunderContent(_contentLocator, crowdfunderType);

        await _contributionRepository.UpdateContributionsCrowdfunderNameAsync(crowdfunderContent, crowdfunderType);
        
        return None.Empty;
    }
}