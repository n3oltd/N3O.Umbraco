using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Members;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding;

public class FundraisingPages : IFundraisingPages {
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IMembersAccessControl _accessControl;

    public FundraisingPages(IMembersAccessControl accessControl, IContentEditor contentEditor, IContentService contentService) {
        _accessControl = accessControl;
        _contentEditor = contentEditor;
        _contentService = contentService;
    }
    
    public async Task<IContentPublisher> GetEditorAsync(Guid id) {
        var content = _contentService.GetById(id);

        if (!content.HasValue()) {
            throw new Exception("Invalid page id");
        }
        
        var canEdit = await _accessControl.CanEditAsync(content);

        if (!canEdit) {
            throw new Exception("Member does not have permisssion to edit the page");
        }

        return _contentEditor.ForExisting(id);
    }
    
    public async Task<IContentPublisher> GetEditorAsync(string name) {
        return _contentEditor.New(name, Guid.Empty, CrowdfundingConstants.CrowdfundingPage.Alias);
    }
}