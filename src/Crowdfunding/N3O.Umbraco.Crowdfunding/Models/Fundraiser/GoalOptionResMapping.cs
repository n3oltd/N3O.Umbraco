using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalOptionResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<CampaignGoalOptionElement, GoalOptionRes>((_, _) => new GoalOptionRes(), Map);
    }

    private void Map(CampaignGoalOptionElement src, GoalOptionRes dest, MapperContext ctx) {
        dest.Id = src.GoalId;
        dest.Name = src.Name;
        dest.Type = src.Type;
        dest.Tags = src.Tags.Select(x => x.Name).ToList();
        dest.Fund = src.Fund.IfNotNull(x => ctx.Map<DonationItem, DonationItemRes>(x.DonationItem));
        dest.Feedback = src.Feedback.IfNotNull(x => ctx.Map<FeedbackScheme, FeedbackSchemeRes>(x.Scheme));
        
        PopulateFundDimensionOptions(ctx, src, dest);
    }

    private void PopulateFundDimensionOptions(MapperContext ctx, CampaignGoalOptionElement src, GoalOptionRes dest) {
        var dimension1 = src.FundDimension1.HasAny() ? src.FundDimension1.ToList() : src.GetFundDimensionOptions().Dimension1Options.ToList();
        var dimension2 = src.FundDimension2.HasAny() ? src.FundDimension2.ToList() : src.GetFundDimensionOptions().Dimension2Options.ToList();
        var dimension3 = src.FundDimension3.HasAny() ? src.FundDimension3.ToList() : src.GetFundDimensionOptions().Dimension3Options.ToList();
        var dimension4 = src.FundDimension4.HasAny() ? src.FundDimension4.ToList() : src.GetFundDimensionOptions().Dimension4Options.ToList();
        
        dest.Dimension1 = GetGoalOptionFundDimensionRes(ctx, dimension1);
        dest.Dimension2 = GetGoalOptionFundDimensionRes(ctx, dimension2);
        dest.Dimension3 = GetGoalOptionFundDimensionRes(ctx, dimension3);
        dest.Dimension4 = GetGoalOptionFundDimensionRes(ctx, dimension4);
    }

    private GoalOptionFundDimensionRes GetGoalOptionFundDimensionRes<TValue>(MapperContext ctx,
                                                                             List<TValue> fundDimensionValues) 
        where TValue : FundDimensionValue<TValue> {
        var res = new GoalOptionFundDimensionRes();
        
        res.Default = fundDimensionValues.FirstOrDefault(x => x.IsUnrestricted)
                                         .IfNotNull(ctx.Map<TValue, FundDimensionValueRes>);
        
        res.AllowedOptions = fundDimensionValues
                            .OrEmpty()
                            .Select(ctx.Map<TValue, FundDimensionValueRes>)
                            .ToList();

        return res;
    }
}