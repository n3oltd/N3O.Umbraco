using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Engage.Lookups;
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
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DeactivateFundraiserHandler(IContentService contentService, ILookups lookups, IWebHostEnvironment webHostEnvironment) {
        _contentService = contentService;
        _lookups = lookups;
        _webHostEnvironment = webHostEnvironment;
    }

    public Task<None> Handle(DeactivateFundraiserCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.FundraiserId.Run(_contentService.GetById, true);
        
        if (_webHostEnvironment.IsProduction()) {
            var currentStatusStr = fundraiser.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Status);
            var currentStatus = _lookups.FindByName<CrowdfunderStatus>(currentStatusStr).Single();

            if (currentStatus.CanToggle &&
                currentStatus.ToggleAction == CrowdfunderActivationActions.Deactivate &&
                currentStatus == CrowdfunderStatuses.Active) {
                fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);

                _contentService.SaveAndPublish(fundraiser);
            }
        } else {
            fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
            fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, CrowdfunderStatuses.Inactive.Name);

            _contentService.SaveAndPublish(fundraiser);
        }

        return Task.FromResult(None.Empty);
    }
}