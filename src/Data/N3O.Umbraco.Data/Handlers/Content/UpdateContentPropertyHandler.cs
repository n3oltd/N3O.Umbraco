using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Handlers;

public class UpdateContentPropertyHandler : IRequestHandler<UpdateContentPropertyCommand, ContentPropertyReq, None> {
    private readonly IContentEditor _contentEditor;

    public UpdateContentPropertyHandler(IContentEditor contentEditor) {
        _contentEditor = contentEditor;
    }
    
    public async Task<None> Handle(UpdateContentPropertyCommand req, CancellationToken cancellationToken) {
        var contentPublisher = req.ContentId.Run(_contentEditor.ForExisting, true);

        await req.Model.Type.UpdatePropertyAsync(contentPublisher.Content, req.Model.Alias, req.Model.Value.Value);

        contentPublisher.SaveAndPublish();
        
        return None.Empty;
    }
}