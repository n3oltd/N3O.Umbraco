using N3O.Umbraco.Content;
using N3O.Umbraco.CrowdFunding;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding;

public class FundraisingPages : IFundraisingPages {
    private readonly FundraisingPageAccessControl _fundraisingPageAccessControl;
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;

    public FundraisingPages(FundraisingPageAccessControl fundraisingPageAccessControl,
                            IContentEditor contentEditor,
                            IContentService contentService,
                            IContentLocator contentLocator) {
        _fundraisingPageAccessControl = fundraisingPageAccessControl;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentLocator = contentLocator;
    }
    
    public async Task<IContentPublisher> GetEditorAsync(Guid id) {
        var content = _contentService.GetById(id);

        var canEdit = await _fundraisingPageAccessControl.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }

        return _contentEditor.ForExisting(id);
    }
    
    public IContentPublisher New(string name) {
        var fundraisingPages = _contentLocator.Single(CrowdfundingConstants.CrowdfundingPages.Alias);
        
        // TODO We should inject IClock and get the current year and month and we should create subfolders for these
        // e.g. 2024 folder with 05 folder inside of it (note padding of month number with leading zero)
        
        // I am not sure we want to take in a name here, it seems to make sense that we initially set the name to something
        // like "[Draft] some-guid-or-whatever" and when the user publishes the page, we have a notification handler
        // which runs which sets the title (like we do for beneficiaries) to the slug of the page and the name so
        // backoffice users can find the page by either.
        
        // noors-special-fundraiser : Noor's special fundraiser
        
        return _contentEditor.New(name, fundraisingPages.Key, CrowdfundingConstants.CrowdfundingPage.Alias);
    }
}