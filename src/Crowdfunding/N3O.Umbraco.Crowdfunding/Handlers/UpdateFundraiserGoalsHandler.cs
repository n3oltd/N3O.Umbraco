using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdateFundraiserGoalsHandler  
    : IRequestHandler<UpdateFundraiserGoalsCommand, UpdateFundraiserGoalsReq, None> {
    private readonly IContentEditor _contentEditor;
    private readonly IContentLocator _contentLocator;

    public UpdateFundraiserGoalsHandler(IContentLocator contentLocator, IContentEditor contentEditor) {
        _contentLocator = contentLocator;
        _contentEditor = contentEditor;
    }
    
    public Task<None> Handle(UpdateFundraiserGoalsCommand req, CancellationToken cancellationToken) {
        var fundraiser = req.ContentId.Run(x => _contentLocator.ById<FundraiserContent>(x), true);
        var contentPublisher =_contentEditor.ForExisting(fundraiser.Content().Key);
        
        contentPublisher.PopulateFundraiserGoals(req.Model.Goals, fundraiser.Campaign);

        contentPublisher.SaveAndPublish();
        
        return Task.FromResult(None.Empty);
    }
}