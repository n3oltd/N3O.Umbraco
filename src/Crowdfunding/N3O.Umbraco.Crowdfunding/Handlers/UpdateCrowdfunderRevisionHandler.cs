using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdateCrowdfunderRevisionHandler : IRequestHandler<UpdateCrowdfunderRevisionCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRevisionRepository _crowdfunderRepository;
    private readonly ILookups _lookups;
    private readonly IContentService _contentService;

    public UpdateCrowdfunderRevisionHandler(IContentLocator contentLocator,
                                            ICrowdfunderRevisionRepository crowdfunderRepository,
                                            ILookups lookups,
                                            IContentService contentService) {
        _contentLocator = contentLocator;
        _crowdfunderRepository = crowdfunderRepository;
        _lookups = lookups;
        _contentService = contentService;
    }

    public async Task<None> Handle(UpdateCrowdfunderRevisionCommand req, CancellationToken cancellationToken) {
        var crowdfunderType = req.TypeId.Run(_lookups.FindById<CrowdfunderType>, true);
        var crowdfunderContent = req.ContentId.Value.ToCrowdfunderContent(_contentLocator, crowdfunderType);
        var version = _contentService.GetById(crowdfunderContent.Key).VersionId;
        
        await _crowdfunderRepository.UpdateCrowdfunderRevisionAsync(crowdfunderContent, version);
        
        return None.Empty;
    }
}