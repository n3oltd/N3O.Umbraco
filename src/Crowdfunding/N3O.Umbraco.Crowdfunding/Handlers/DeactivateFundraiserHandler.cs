using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class DeactivateFundraiserHandler : IRequestHandler<DeactivateFundraiserCommand, None, None> {
    private readonly IContentService _contentService;
    private readonly ILookups _lookups;

    public DeactivateFundraiserHandler(IContentService contentService, ILookups lookups) {
        _contentService = contentService;
        _lookups = lookups;
    }

    public Task<None> Handle(DeactivateFundraiserCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.FundraiserId.Run(_contentService.GetById, true);

        var currentStatusStr = fundraiser.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Status);
        var currentStatus = _lookups.FindByName<CrowdfunderStatus>(currentStatusStr).Single();

        if (currentStatus.CanToggle &&
            currentStatus.ToggleAction == CrowdfunderActivationActions.Deactivate &&
            currentStatus == CrowdfunderStatuses.Active) {
            fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);

            _contentService.SaveAndPublish(fundraiser);
        }

        return Task.FromResult(None.Empty);
    }
}