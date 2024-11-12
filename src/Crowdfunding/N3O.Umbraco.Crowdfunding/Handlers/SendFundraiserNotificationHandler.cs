using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content.Templates;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Content;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class SendFundraiserNotificationHandler :
    IRequestHandler<SendFundraiserNotificationCommand, FundraiserNotificationReq, None> {
    private readonly IContentCache _contentCache;
    private readonly IEmailBuilder _emailBuilder;

    public SendFundraiserNotificationHandler(IContentCache contentCache, IEmailBuilder emailBuilder) {
        _contentCache = contentCache;
        _emailBuilder = emailBuilder;
    }
    
    public Task<None> Handle(SendFundraiserNotificationCommand req, CancellationToken cancellationToken) {
        if (req.Model.Type == FundraiserNotificationTypes.FundraiserCreated) {
            SendEmail<FundraiserCreatedTemplateContent>(req.Model);
        } else if (req.Model.Type == FundraiserNotificationTypes.StillDraft) {
            SendEmail<StillDraftTemplateContent>(req.Model);
        } else if (req.Model.Type == FundraiserNotificationTypes.GoalsCompleted) {
            SendEmail<GoalsCompletedTemplateContent>(req.Model);
        } else if (req.Model.Type == FundraiserNotificationTypes.GoalsExceeded) {
            SendEmail<StillDraftTemplateContent>(req.Model);
        } else if (req.Model.Type == FundraiserNotificationTypes.StillDraft) {
            SendEmail<StillDraftTemplateContent>(req.Model);
        } else {
            throw UnrecognisedValueException.For(req.Model.Type);
        }
        
        return Task.FromResult(None.Empty);
    }

    private void SendEmail<T>(FundraiserNotificationReq req) where T : EmailTemplateContent<T> {
        var template = _contentCache.Single<T>();

        if (template.HasValue()) {
            _emailBuilder.QueueTemplate(template, req.Fundraiser.FundraiserEmail, req.Fundraiser);
        }
    }
}