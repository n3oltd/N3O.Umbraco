using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingHelper : ICrowdfundingHelper {
    private IPublishedContent _rootPage;
    private string _rootPath;
    
    private readonly FundraiserAccessControl _fundraiserAccessControl;
    private readonly IContentCache _contentCache;
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly IMemberManager _memberManager;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IForexConverter _forexConverter;

    public CrowdfundingHelper(FundraiserAccessControl fundraiserAccessControl,
                              IContentCache contentCache,
                              IContentEditor contentEditor,
                              IContentService contentService,
                              IContentLocator contentLocator,
                              IMemberManager memberManager,
                              ICurrencyAccessor currencyAccessor,
                              IForexConverter forexConverter) {
        _fundraiserAccessControl = fundraiserAccessControl;
        _contentCache = contentCache;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentLocator = contentLocator;
        _memberManager = memberManager;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
    }
    
    public string GetCrowdfundingPath(Uri requestUri) {
        var rootPath = GetRootPath();
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant();
        
        if (requestedPath.StartsWith(rootPath)) {
            return requestedPath.Substring(rootPath.Length + 1);
        } else {
            return null;
        }
    }
    
    public IPublishedContent GetRootPage() {
        if (_rootPage == null) {
            _rootPage = _contentCache.Single(CrowdfundingConstants.Root.Alias);
        }

        return _rootPage;
    }
    
    public string GetRootPath() {
        if (_rootPath == null) {
            var rootPage = GetRootPage();
            
            if (rootPage.HasValue()) {
                _rootPath = _rootPage.RelativeUrl().TrimEnd("/");
            }
        }

        return _rootPath;
    }
    
    public async Task<CreateFundraiserResult> CreateFundraiserAsync(CreateFundraiserCommand req) {
        var fundraisingPages = _contentLocator.Single(CrowdfundingConstants.Fundraisers.Alias);

         var contentPublisher =_contentEditor.New(Guid.NewGuid().ToString(),
                                                  fundraisingPages.Key,
                                                  CrowdfundingConstants.CrowdfundingPage.Alias);
         
         var member = await MemberExtensions.GetCurrentMemberAsync(_memberManager);
         
         contentPublisher.SetContentValues(_contentLocator, req.Model, member);
        
        var publishResult = contentPublisher.SaveAndPublish();

        if (publishResult.Success) {
            var publishedContent = _contentLocator.ById<FundraiserContent>(publishResult.Content.Key);

            return CreateFundraiserResult.ForSuccess(publishedContent);
        } else {
            return CreateFundraiserResult.ForError(publishResult.EventMessages.OrEmpty(x => x.GetAll()));
        }
    }

    public async Task<IContentPublisher> GetEditorAsync(Guid id) {
        var content = _contentService.GetById(id);

        var canEdit = await _fundraiserAccessControl.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }

        return _contentEditor.ForExisting(id);
    }
    
    public Money GetQuoteMoney(decimal amount) {
        var currency = _currencyAccessor.GetCurrency();

        return _forexConverter.BaseToQuote().ToCurrency(currency).Convert(amount).Quote;
    }
    
    public IReadOnlyList<FundraiserContent> GetAllFundraisers() {
        var fundraisingPages = _contentLocator.All<FundraiserContent>();

        return fundraisingPages;
    }
    
    public bool IsFundraiser(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);
    }

    public bool IsFundraiserNameAvailable(string name) {
        var allFundraisers = GetAllFundraisers();

        return allFundraisers.All(x => !x.Title.EqualsInvariant(name));
    }
}