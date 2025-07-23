using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalOptionResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public GoalOptionResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<CampaignGoalOptionElement, GoalOptionRes>((_, _) => new GoalOptionRes(), Map);
    }

    private void Map(CampaignGoalOptionElement src, GoalOptionRes dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.Type = src.Type;
        dest.Tags = src.Tags.OrEmpty().Select(ctx.Map<TagContent, TagRes>);
        dest.Fund = src.Fund.IfNotNull(x => ctx.Map<DonationItem, DonationItemRes>(x.GetDonationItem(_lookups)));
        dest.Feedback = src.Feedback.IfNotNull(x => ctx.Map<FeedbackScheme, FeedbackSchemeRes>(x.GetScheme(_lookups)));
        
        PopulateFundDimensionOptions(ctx, src, dest);
    }

    private void PopulateFundDimensionOptions(MapperContext ctx, CampaignGoalOptionElement src, GoalOptionRes dest) {
        var fundDimension1Values = src.GetDimension1Values(_lookups).ToList();
        var fundDimension2Values = src.GetDimension2Values(_lookups).ToList();
        var fundDimension3Values = src.GetDimension3Values(_lookups).ToList();
        var fundDimension4Values = src.GetDimension4Values(_lookups).ToList();

        var fundDimensionOptions = src.GetFundDimensionOptions(_lookups);
        
        var dimension1 = fundDimension1Values.HasAny() ? fundDimension1Values : fundDimensionOptions.Dimension1.ToList();
        var dimension2 = fundDimension2Values.HasAny() ? fundDimension2Values : fundDimensionOptions.Dimension2.ToList();
        var dimension3 = fundDimension3Values.HasAny() ? fundDimension3Values : fundDimensionOptions.Dimension3.ToList();
        var dimension4 = fundDimension4Values.HasAny() ? fundDimension4Values : fundDimensionOptions.Dimension4.ToList();
        
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
        
        res.AllowedOptions = fundDimensionValues.OrEmpty().Select(ctx.Map<TValue, FundDimensionValueRes>).ToList();

        return res;
    }
}