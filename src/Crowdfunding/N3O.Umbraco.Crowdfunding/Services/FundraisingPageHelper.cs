using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingPageHelper {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingContributionRepository _repository;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IForexConverter _forexConverter;
    
    public FundraisingPageHelper(IContentLocator contentLocator,
                                 ICrowdfundingContributionRepository repository,
                                 ICurrencyAccessor currencyAccessor,
                                 IForexConverter forexConverter) {
        _contentLocator = contentLocator;
        _repository = repository;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
    }
    
    public async Task<IReadOnlyList<CrowdfundingContribution>> GetContributionsForPageAsync(Guid pageId) {
        return await GetContributionsForPagesAsync(pageId.Yield());
    }
    
    public async Task<IReadOnlyList<CrowdfundingContribution>> GetContributionsForPagesAsync(IEnumerable<Guid> pageIds) {
        var query = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.CrowdfundingContributions.Name}"); //WHERE PageId = '{pageIds.ToString()}'
        
        var contributions = await _repository.FetchContributionsAsync(query);

        return contributions.ToList();
    }
    
    public IReadOnlyList<CrowdfundingPageContent> GetAllPagesForCampaign(Guid campaignId) {
        var pages = _contentLocator.All<CrowdfundingPageContent>().Where(x => x.Campaign.Content().Key == campaignId);

        return pages.ToList();
    }
    
    public Money GetQuoteMoney(decimal amount) {
        var currency = _currencyAccessor.GetCurrency();

        return _forexConverter.BaseToQuote().ToCurrency(currency).Convert(amount).Quote;
    }
}