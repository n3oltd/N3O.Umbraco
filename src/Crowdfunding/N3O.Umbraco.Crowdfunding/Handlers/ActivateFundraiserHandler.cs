using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class ActivateFundraiserHandler :
    IRequestHandler<ActivateFundraiserCommand, None, None> {
    private readonly IContentService _contentService;
    private readonly ILookups _lookups;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ActivateFundraiserHandler(IContentService contentService, ILookups lookups, IWebHostEnvironment webHostEnvironment) {
        _contentService = contentService;
        _lookups = lookups;
        _webHostEnvironment = webHostEnvironment;
    }

    public Task<None> Handle(ActivateFundraiserCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.FundraiserId.Run(_contentService.GetById, true);
        
        if (_webHostEnvironment.IsProduction()) {
            var currentStatusStr = fundraiser.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Status);
            var currentStatus = _lookups.FindByName<CrowdfunderStatus>(currentStatusStr).Single();

            if (currentStatus.CanToggle &&
                currentStatus.ToggleAction == CrowdfunderActivationActions.Activate &&
                currentStatus != CrowdfunderStatuses.Active) {
                fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);

                _contentService.SaveAndPublish(fundraiser);
            }
        } else {
            fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
            fundraiser.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, CrowdfunderStatuses.Active.Name);

            _contentService.SaveAndPublish(fundraiser);
        }

        return Task.FromResult(None.Empty);
    }
}