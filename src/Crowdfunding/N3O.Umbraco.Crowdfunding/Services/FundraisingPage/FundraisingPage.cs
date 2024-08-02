using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models.FundraisingPage;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingPage : FundraisingPageBase {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingContributionRepository _crowdfundingContributionRepository;
    
    public FundraisingPage(IContentLocator contentLocator,
                           ICrowdfundingContributionRepository crowdfundingContributionRepository) {
        _contentLocator = contentLocator;
        _crowdfundingContributionRepository = crowdfundingContributionRepository;
    }
    
    public override bool IsMatch(string path) {
        return Regex.IsMatch(path, FundraisingPageUrl.FundraisingPagePathPattern, RegexOptions.IgnoreCase);
    }

    public override async Task<object> GetViewModelAsync(string path) {
        var match = Regex.Match(path, FundraisingPageUrl.FundraisingPagePathPattern);
        var pageId = int.Parse(match.Groups[1].Value);
        var pageContent = _contentLocator.ById<CrowdfundingPageContent>(pageId);

        var viewModel = new CrowdfundingPageViewModel();
        viewModel.Content = pageContent;
        viewModel.Contributions = await GetContributionsViewModel(pageContent.Content().Key);

        return viewModel;
    }

    private async Task<IReadOnlyList<CrowdfundingContributionViewModel>> GetContributionsViewModel(Guid pageId) {
        var contributions = await _crowdfundingContributionRepository.GetAllContributionsForPageAsync(pageId);

        var model = contributions.Select(x => new CrowdfundingContributionViewModel {
                                                                                        Name = x.Name,
                                                                                        Date = x.Timestamp,
                                                                                        Amount = new Money(x.QuoteAmount, new Currency()),
                                                                                    });

        return model.ToList();
    }
}