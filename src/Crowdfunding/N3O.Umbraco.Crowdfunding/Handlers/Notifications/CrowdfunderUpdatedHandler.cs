using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CrowdfunderUpdatedHandler : IRequestHandler<CrowdfunderUpdatedNotification, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ICrowdfunderRevisionRepository _crowdfunderRevisionRepository;
    private readonly IContributionRepository _contributionRepository;
    private readonly IContentService _contentService;
    private readonly ILookups _lookups;

    public CrowdfunderUpdatedHandler(IContentLocator contentLocator,
                                     ICrowdfunderRepository crowdfunderRepository,
                                     ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                     IContributionRepository contributionRepository,
                                     IContentService contentService,
                                     ILookups lookups) {
        _contentLocator = contentLocator;
        _crowdfunderRepository = crowdfunderRepository;
        _crowdfunderRevisionRepository = crowdfunderRevisionRepository;
        _contributionRepository = contributionRepository;
        _contentService = contentService;
        _lookups = lookups;
    }

    public async Task<None> Handle(CrowdfunderUpdatedNotification req, CancellationToken cancellationToken) {
        var type = req.TypeId.Run(_lookups.FindById<CrowdfunderType>, true);
        var content = req.ContentId.Run(id => _contentLocator.GetCrowdfunderContent(id, type), true);
        var version = _contentService.GetById(content.Key).VersionId;
        
        await _crowdfunderRepository.AddOrUpdateAsync(content);
        await _crowdfunderRevisionRepository.AddOrUpdateAsync(content, version);
        await _contributionRepository.UpdateCrowdfunderNameAsync(content, type);
        
        return None.Empty;
    }
}