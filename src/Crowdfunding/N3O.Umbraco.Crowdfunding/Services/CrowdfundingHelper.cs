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

namespace N3O.Umbraco.Crowdfunding;

public partial class CrowdfundingHelper : ICrowdfundingHelper {
    private readonly Lazy<FundraiserAccessControl> _fundraiserAccessControl;
    private readonly Lazy<IContentEditor> _contentEditor;
    private readonly Lazy<IContentService> _contentService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IMemberManager> _memberManager;
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;
    private readonly Lazy<IForexConverter> _forexConverter;

    public CrowdfundingHelper(Lazy<FundraiserAccessControl> fundraiserAccessControl,
                              Lazy<IContentEditor> contentEditor,
                              Lazy<IContentService> contentService,
                              Lazy<IContentLocator> contentLocator,
                              Lazy<IMemberManager> memberManager,
                              Lazy<ICurrencyAccessor> currencyAccessor,
                              Lazy<IForexConverter> forexConverter) {
        _fundraiserAccessControl = fundraiserAccessControl;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentLocator = contentLocator;
        _memberManager = memberManager;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
    }
    
    public async Task<CreateFundraiserResult> CreateFundraiserAsync(CreateFundraiserCommand req) {
        var fundraisersCollection = _contentLocator.Value.Single(CrowdfundingConstants.Fundraisers.Alias);

         var contentPublisher =_contentEditor.Value.New(Guid.NewGuid().ToString(),
                                                        fundraisersCollection.Key,
                                                        CrowdfundingConstants.Fundraiser.Alias);
         
         var member = await MemberExtensions.GetCurrentMemberAsync(_memberManager.Value);
         
         contentPublisher.PopulateFundraiser(_contentLocator.Value, req.Model, member);
        
        var publishResult = contentPublisher.SaveAndPublish();

        if (publishResult.Success) {
            var publishedContent = _contentLocator.Value.ById<FundraiserContent>(publishResult.Content.Key);

            return CreateFundraiserResult.ForSuccess(publishedContent);
        } else {
            return CreateFundraiserResult.ForError(publishResult.EventMessages.OrEmpty(x => x.GetAll()));
        }
    }
    
    public IPublishedContent GetCrowdfundingHomePage() {
        return GetCrowdfundingHomePage(_contentLocator.Value);
    }

    public string GetCrowdfundingPath(Uri requestUri) {
        return GetCrowdfundingPath(_contentLocator.Value, requestUri);
    }
    
    public IReadOnlyList<FundraiserContent> GetAllFundraisers() {
        var allFundraisers = _contentLocator.Value.All<FundraiserContent>();

        return allFundraisers;
    }
    
    public async Task<IContentPublisher> GetEditorAsync(Guid id) {
        var content = _contentService.Value.GetById(id);

        var canEdit = await _fundraiserAccessControl.Value.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }

        return _contentEditor.Value.ForExisting(id);
    }

    public Money GetQuoteMoney(decimal baseAmount) {
        var currency = _currencyAccessor.Value.GetCurrency();

        return _forexConverter.Value.BaseToQuote().ToCurrency(currency).Convert(baseAmount).Quote;
    }
    
    public bool IsFundraiser(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);
    }

    public bool IsFundraiserTitleAvailable(string title) {
        var allFundraisers = GetAllFundraisers();

        return allFundraisers.All(x => !x.Title.EqualsInvariant(title));
    }
}