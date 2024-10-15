using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class AddOrUpdateCrowdfunderHandler : IRequestHandler<AddOrUpdateCrowdfunderCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ILookups _lookups;

    public AddOrUpdateCrowdfunderHandler(IContentLocator contentLocator,
                                    ICrowdfunderRepository crowdfunderRepository,
                                    ILookups lookups) {
        _contentLocator = contentLocator;
        _crowdfunderRepository = crowdfunderRepository;
        _lookups = lookups;
    }

    public async Task<None> Handle(AddOrUpdateCrowdfunderCommand req, CancellationToken cancellationToken) {
        var crowdfunderType = req.TypeId.Run(_lookups.FindById<CrowdfunderType>, true);
        var crowdfunderContent = req.ContentId.Value.ToCrowdfunderContent(_contentLocator, crowdfunderType);
        
        await _crowdfunderRepository.AddOrUpdateCrowdfunderAsync(crowdfunderContent);
        
        return None.Empty;
    }
}