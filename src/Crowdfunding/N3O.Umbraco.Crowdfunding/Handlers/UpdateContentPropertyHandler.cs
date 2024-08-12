using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdateContentPropertyHandler : IRequestHandler<UpdateContentPropertyCommand, ContentPropertyReq, None> {
    private readonly ICrowdfundingHelper _crowdfundingHelper;

    public UpdateContentPropertyHandler(ICrowdfundingHelper crowdfundingHelper) {
        _crowdfundingHelper = crowdfundingHelper;
    }
    
    public async Task<None> Handle(UpdateContentPropertyCommand req, CancellationToken cancellationToken) {
        var contentPublisher = await req.ContentId.RunAsync(id => _crowdfundingHelper.GetEditorAsync(id), true);

        await req.Model.Type.UpdatePropertyAsync(contentPublisher.Content, req.Model.Alias, req.Model.Value.Value);

        contentPublisher.SaveAndPublish();
        
        return None.Empty;
    }
}