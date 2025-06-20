using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDesignationMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;
    
    public PublishedDesignationMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DesignationContent, PublishedDesignation>((_, _) => new PublishedDesignation(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DesignationContent src, PublishedDesignation dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = (DesignationType) Enum.Parse(typeof(DesignationType), src.Type.Id, true);
        dest.Name = src.Name;
        dest.Image = new Uri(src.Image.GetCropUrl(urlMode: UrlMode.Absolute));
        dest.Icon = new Uri(src.Image.GetCropUrl(urlMode: UrlMode.Absolute));
        dest.ShortDescription = src.ShortDescription.ToHtmlString();
        dest.LongDescription = src.LongDescription.ToHtmlString();
        dest.SuggestedGiftType = (GiftType) Enum.Parse(typeof(GiftType), src.SuggestedGiftType.Id, true);
        
        dest.GiftTypes = src.GetAllowedGivingTypes().Select(GetGiftType).ToList(); // TODO
        
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackDesignationContent, PublishedFeedbackDesignation>);
        dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDesignationContent, PublishedFundDesignation>);
        dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDesignationContent, PublishedSponsorshipDesignation>);
            
        dest.FundDimensions = GetPublishedDesignationFundDimensions(src, src.GetFundDimensionOptions());
    }
    
    private PublishedDesignationFundDimensions GetPublishedDesignationFundDimensions(DesignationContent src, 
                                                                                     IFundDimensionsOptions fundDimensionOptions) {
        var fundDimensions = new PublishedDesignationFundDimensions();
        fundDimensions.Dimension1 = PublishedDesignationFundDimension(src.Dimension1,
                                                                      fundDimensionOptions.Dimension1Options);
        
        fundDimensions.Dimension2 = PublishedDesignationFundDimension(src.Dimension2,
                                                                      fundDimensionOptions.Dimension2Options);
        
        fundDimensions.Dimension3 = PublishedDesignationFundDimension(src.Dimension3,
                                                                      fundDimensionOptions.Dimension3Options);


        return fundDimensions;
    }
    
    private PublishedDesignationFundDimension PublishedDesignationFundDimension(IFundDimensionValue @fixed,
                                                                                IEnumerable<IFundDimensionValue> fundDimensionValues) {
        var designationFundDimension = new PublishedDesignationFundDimension();
        designationFundDimension.Options = fundDimensionValues.OrEmpty().Select(x => x.Name).ToList();
        designationFundDimension.Fixed = @fixed?.Name;
        
        if (designationFundDimension.Fixed == null) {
            var values = fundDimensionValues.OrEmpty().ToList();

            var unrestricted = values.FirstOrDefault(x => x.IsUnrestricted);

            designationFundDimension.Suggested = unrestricted?.Name ?? values.FirstOrDefault()?.Name;
        }

        return designationFundDimension;
    }
    
    private GiftType GetGiftType(GivingType givingType) {
        if (givingType == GivingTypes.Donation) {
            return GiftType.OneTime;
        } else {
            return GiftType.Recurring;
        }
    }
}