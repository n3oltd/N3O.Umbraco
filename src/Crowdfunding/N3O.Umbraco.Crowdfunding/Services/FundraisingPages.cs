using N3O.Umbraco.Content;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
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

    // TODO It does not really make sense to create a completely blank page. According to Figma, at the stage where
    // we are creating the "barebones" page for the user to fill in a small number of selections have already been
    // made. E.g. we know the page owner (logged in user), we know the campaign, possibly allocation etc. Therefore
    // this method should have parameters for these values, and the calling controller/handler should have a request
    // model where these are passed in from the frontend.
    public CreatePageResult CreatePage() {
        var fundraisingPages = _contentLocator.Single(CrowdfundingConstants.CrowdfundingPages.Alias);
        
        var content = _contentService.Create(Guid.NewGuid().ToString(),
                                             fundraisingPages.Key,
                                             CrowdfundingConstants.CrowdfundingPage.Alias);

        var publishResult = _contentService.SaveAndPublish(content);

        if (publishResult.Success) {
            var publishedContent = _contentLocator.ById<CrowdfundingPageContent>(publishResult.Content.Key);

            return CreatePageResult.ForSuccess(publishedContent);
        } else {
            return CreatePageResult.ForError(publishResult.EventMessages.OrEmpty(x => x.GetAll()));
        }
    }

    public async Task<IContentPublisher> GetEditorAsync(Guid id) {
        var content = _contentService.GetById(id);

        var canEdit = await _fundraisingPageAccessControl.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }

        return _contentEditor.ForExisting(id);
    }

    // TODO Implement this method
    public bool IsPageNameAvailable(string name) {
        return false;
    }
}