using N3O.Umbraco.Content;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding;

public class FundraisingPages : IFundraisingPages {
    private readonly FundraisingPageAccessControl _fundraisingPageAccessControl;
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly IMemberManager _memberManager;

    public FundraisingPages(FundraisingPageAccessControl fundraisingPageAccessControl,
                            IContentEditor contentEditor,
                            IContentService contentService,
                            IContentLocator contentLocator,
                            IMemberManager memberManager) {
        _fundraisingPageAccessControl = fundraisingPageAccessControl;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentLocator = contentLocator;
        _memberManager = memberManager;
    }
    
    public async Task<CreatePageResult> CreatePageAsync(CreatePageCommand req) {
        var fundraisingPages = _contentLocator.Single(CrowdfundingPages.Alias);

         var contentPublisher =_contentEditor.New(Guid.NewGuid().ToString(),
                                                  fundraisingPages.Key,
                                                  CrowdfundingPage.Alias);
         
         var member = await MemberExtensions.GetCurrentMemberAsync(_memberManager);
         
         contentPublisher.SetContentValues(_contentLocator, req.Model, member);
        
        var publishResult = contentPublisher.SaveAndPublish();

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
    
    public IReadOnlyList<CrowdfundingPageContent> GetAllFundraisingPages() {
        var fundraisingPages = _contentLocator.All<CrowdfundingPageContent>();

        return fundraisingPages;
    }
    
    public bool IsFundraisingPage(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingPage.Alias);
    }

    public bool IsPageNameAvailable(string name) {
        var pages = GetAllFundraisingPages();

        return pages.All(x => x.PageTitle != name);
    }
}