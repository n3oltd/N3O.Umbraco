using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdatePagePropertyHandler : IRequestHandler<UpdatePagePropertyCommand, PagePropertyReq, None> {
    private readonly IFundraisingPages _fundraisingPages;

    public UpdatePagePropertyHandler(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    public async Task<None> Handle(UpdatePagePropertyCommand req, CancellationToken cancellationToken) {
        var contentPublisher = await req.PageId.RunAsync(id => _fundraisingPages.GetEditorAsync(id), true);

        await req.Model.Type.UpdatePropertyAsync(contentPublisher, req.Model.Alias, req.Model.Value.Value);

        contentPublisher.SaveAndPublish();
        
        return None.Empty;
    }
}