using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

[ApiDocument(PlatformsConstants.DevToolsApiName)]
public class PlatformsDevToolsController : BackofficeAuthorizedApiController {
    private const string CampaignsWebhookId = PlatformsConstants.WebhookIds.Campaigns;
    private const string OfferingsWebhookId = PlatformsConstants.WebhookIds.Offerings;
    private const string DonationButtonsWebhookId = PlatformsConstants.WebhookIds.DonationButtons;
    private const string DonationFormsWebhookId = PlatformsConstants.WebhookIds.DonationForms;
    private const string DonationPopupsWebhookId = PlatformsConstants.WebhookIds.DonationPopups;
    
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly ICloudUrl _cloudUrl;
    private readonly IBackgroundJob _backgroundJob;

    public PlatformsDevToolsController(IContentLocator contentLocator, IUmbracoMapper mapper, ICloudUrl cloudUrl, IBackgroundJob backgroundJob) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _cloudUrl = cloudUrl;
        _backgroundJob = backgroundJob;
    }

    [HttpPost("webhooks/resend/campaigns/all")]
    public Task<ActionResult> ResendCampaignsWebhooks() {
        var campaigns =  _contentLocator.All(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias())).As<CampaignContent>();
        
        foreach (var campaign in campaigns) {
            var body = _mapper.Map<CampaignContent, CampaignWebhookBodyReq>(campaign);
            
            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(CampaignsWebhookId);
                
            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, CampaignsWebhookId);
        }
        
        return Task.FromResult<ActionResult>(Ok());
    }
    
    [HttpPost("webhooks/resend/offerings/all")]
    public Task<ActionResult> ResendOfferingsWebhooks() {
        var offerings =  _contentLocator.All(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias())).As<OfferingContent>();
        
        foreach (var offering in offerings) {
            var body = _mapper.Map<OfferingContent, OfferingWebhookBodyReq>(offering);
            
            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(OfferingsWebhookId);
                
            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, OfferingsWebhookId);
        }
        
        return Task.FromResult<ActionResult>(Ok());
    }
    
    [HttpPost("webhooks/resend/elements/all")]
    public Task<ActionResult> ResendElementsWebhooks() {
        var elements =  _contentLocator.All(x => x.IsComposedOf(AliasHelper<ElementContent>.ContentTypeAlias())).As<ElementContent>();
        
        foreach (var element in elements.Where(x => x.Type == ElementTypes.DonationButton)) {
            var body = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationButtonReq>(element);
            
            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(DonationButtonsWebhookId);
                
            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, DonationButtonsWebhookId);
        }
        
        foreach (var element in elements.Where(x => x.Type == ElementTypes.DonationForm)) {
            var body = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationFormReq>(element);
            
            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(DonationFormsWebhookId);
                
            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, DonationFormsWebhookId);
        }
        
        foreach (var element in elements.Where(x => x.Type == ElementTypes.DonationPopup)) {
            var body = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationPopupReq>(element);
            
            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(DonationPopupsWebhookId);
                
            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, DonationPopupsWebhookId);
        }
        
        return Task.FromResult<ActionResult>(Ok());
    }
}