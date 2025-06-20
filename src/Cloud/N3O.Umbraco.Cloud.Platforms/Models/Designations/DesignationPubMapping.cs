using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDesignationMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;
    
    public PublishedDesignationMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IDesignation, PublishedDesignation>((_, _) => new PublishedDesignation(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IDesignation src, PublishedDesignation dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = GetDesignationType(src);
        dest.Name = src.Name;
        dest.Image = new Uri(_urlBuilder.Root().AppendPathSegment(src.Image.SrcUrl()));
        dest.Icon = new Uri(_urlBuilder.Root().AppendPathSegment(src.Image.SrcUrl()));
        dest.ShortDescription = src.ShortDescription.ToHtmlString();
        dest.LongDescription = src.LongDescription.ToHtmlString();
        dest.SuggestedGiftType = src.SuggestedGiftType.IfNotNull(GetGiftType);
        dest.GiftTypes = GetAllowedGiftTypes(src);
        
        if (src is FeedbackDesignation feedbackDesignation) {
            dest.Feedback = ctx.Map<IDesignation, PublishedFeedbackDesignation>(src);
            
            dest.FundDimensions = GetPublishedDesignationFundDimensions(src, feedbackDesignation.Scheme);
        } else if (src is FundDesignation fundDesignation) {
            dest.Fund = ctx.Map<IDesignation, PublishedFundDesignation>(src);
            
            dest.FundDimensions = GetPublishedDesignationFundDimensions(src, fundDesignation.DonationItem);
        } else if (src is SponsorshipDesignation sponsorshipDesignation) {
            dest.Sponsorship = ctx.Map<IDesignation, PublishedSponsorshipDesignation>(src);
            
            dest.FundDimensions = GetPublishedDesignationFundDimensions(src, sponsorshipDesignation.Scheme);
        } else {
            throw UnrecognisedValueException.For(src.ContentType.Alias);
        }
    }
    
    private List<GiftType> GetAllowedGiftTypes(IDesignation designation) {
        if (designation is FundDesignation fundDesignation) {
            return fundDesignation.DonationItem.AllowedGivingTypes.Select(GetGiftType).ToList();
        } else if (designation is FeedbackDesignation feedbackDesignation) {
            return feedbackDesignation.Scheme.AllowedGivingTypes.Select(GetGiftType).ToList();
        } else if (designation is SponsorshipDesignation sponsorshipDesignation) {
            return sponsorshipDesignation.Scheme.AllowedGivingTypes.Select(GetGiftType).ToList();
        } else {
            throw UnrecognisedValueException.For(designation.ContentType.Alias);
        }
    }
    
    private DesignationType GetDesignationType(IDesignation designation) {
        if (designation is FundDesignation) {
            return DesignationType.Fund;
        } else if (designation is FeedbackDesignation) {
            return DesignationType.Feedback;
        } else if (designation is SponsorshipDesignation) {
            return DesignationType.Sponsorship;
        } else {
            throw UnrecognisedValueException.For(designation.ContentType.Alias);
        }
    }
    
    private PublishedDesignationFundDimensions GetPublishedDesignationFundDimensions(IDesignation src, 
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