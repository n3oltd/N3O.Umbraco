using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsResMapping : IMapDefinition {
    private readonly IForexConverter _forexConverter;
    
    public FundraiserGoalsResMapping(IForexConverter forexConverter) {
        _forexConverter = forexConverter;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundraiserContent, FundraiserGoalsRes>((_, _) => new FundraiserGoalsRes(), Map);
    }

    private void Map(FundraiserContent src, FundraiserGoalsRes dest, MapperContext ctx) {
        var value = _forexConverter.QuoteToBase()
                                   .FromCurrency(src.Campaign.Currency)
                                   .UsingRateOn(src.Campaign.Content().CreateDate.ToLocalDate())
                                   .ConvertAsync(src.Campaign.MinimumAmount)
                                   .GetAwaiter()
                                   .GetResult();
        
        dest.Currency = ctx.Map<Currency, CurrencyRes>(src.Currency);
        dest.MinimumValues = ctx.Map<decimal, Dictionary<string, MoneyRes>>(value.Base.Amount);
        dest.GoalOptions = src.Campaign.GoalOptions.Select(ctx.Map<CampaignGoalOptionElement, GoalOptionRes>);
        dest.SelectedGoals = src.Goals.Select(ctx.Map<GoalElement, GoalRes>);
    }
}