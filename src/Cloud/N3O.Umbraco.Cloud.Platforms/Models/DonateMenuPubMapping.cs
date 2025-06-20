using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDonateMenuMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public PublishedDonateMenuMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsContent, PublishedDonateMenu>((_, _) => new PublishedDonateMenu(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(PlatformsContent src, PublishedDonateMenu dest, MapperContext ctx) {
        var entries = new List<PublishedDonateMenuEntry>();

        var allCampaigns = src.Descendants()
                              .Where(x => x.IsComposedOf(AliasHelper<Campaign>.ContentTypeAlias()))
                              .As<ICampaign>();
        
        var telethonEntries = GetTelethonEntries(ctx, allCampaigns);
        var standardEntries = GetStandardEntries(ctx, allCampaigns);
        var browsableEntries = GetBrowsableEntries(allCampaigns);

        entries.AddRange(telethonEntries);
        entries.Add(standardEntries);
        entries.AddRange(browsableEntries);
        
        dest.Entries = entries;
    }

    private PublishedDonateMenuEntry GetStandardEntries(MapperContext ctx, IEnumerable<ICampaign> campaigns) {
        var standardCampaigns = campaigns.Where(x => x is StandardCampaign).As<StandardCampaign>();
        
        var standardEntry = new PublishedDonateMenuEntry();
        standardEntry.Type = DonateMenuEntryType.CampaignsList;
        standardEntry.Name = "Campaigns";
        standardEntry.Icon = null; // TODO
        standardEntry.CampaignsList = new PublishedDonateMenuCampaignsListEntry();
        standardEntry.CampaignsList.Campaigns = standardCampaigns.Select(ctx.Map<ICampaign, PublishedCampaignSummary>).ToList();
        
        return standardEntry;
    }

    private IEnumerable<PublishedDonateMenuEntry> GetTelethonEntries(MapperContext ctx, IEnumerable<ICampaign> campaigns) {
        var telethonCampaigns = campaigns.Where(x => x is TelethonCampaign).As<TelethonCampaign>().OrEmpty();

        var telethonCampaignsPub = new List<PublishedDonateMenuEntry>();
        
        foreach (var telethonCampaign in telethonCampaigns) {
            var telethonEntry = new PublishedDonateMenuEntry();
            telethonEntry.Type = DonateMenuEntryType.Campaign;
            telethonEntry.Name = telethonCampaign.Name;
            telethonEntry.Icon = new Uri(telethonCampaign.Image.GetCropUrl("m"));
            telethonEntry.Campaign = ctx.Map<ICampaign, PublishedCampaign>(telethonCampaign);
                
            telethonCampaignsPub.Add(telethonEntry);
        }

        return telethonCampaignsPub;
    }

    private IEnumerable<PublishedDonateMenuEntry> GetBrowsableEntries(IReadOnlyList<ICampaign> allCampaigns) {
        var allDesignations = allCampaigns.SelectMany(x => x.Descendants().Where(y => y.IsComposedOf(AliasHelper<Designation>.ContentTypeAlias()))
                                                            .As<IDesignation>())
                                          .ToList();
        
        var entries = new List<PublishedDonateMenuEntry>();
        
        var fundDimensions = _contentLocator.Single<PlatformsFundStructure>();
        
        var fundDimension1 = fundDimensions.Child<PlatformsFundDimension1>();
        var fundDimension2 = fundDimensions.Child<PlatformsFundDimension2>();
        var fundDimension3 = fundDimensions.Child<PlatformsFundDimension3>();
        var fundDimension4 = fundDimensions.Child<PlatformsFundDimension4>();

        if (fundDimension1?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimension1, 1));
        }
        
        if (fundDimension2?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimension2, 2));
        }
        
        if (fundDimension3?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimension3, 3));
        }
        
        if (fundDimension4?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimension4, 4));
        }
        
        return entries;
    }

    private PublishedDonateMenuEntry GetBrowsableEntry(IEnumerable<IDesignation> allDesignations,
                                                       IPlatformsFundDimension fundDimension,
                                                       int dimensionNumber) {
        var entry = new PublishedDonateMenuEntry();
        entry.Type = DonateMenuEntryType.FundDimensionSearch;
        entry.Name = fundDimension.Name;
        entry.Icon = null; //TODO
        
        entry.FundDimensionSearch = new PublishedDonateMenuFundDimensionSearchEntry();
        entry.FundDimensionSearch.DimensionNumber = dimensionNumber;
        entry.FundDimensionSearch.Values = GetFundDimensionValues(allDesignations, fundDimension);
        
        return entry;
    }
    
    private List<string> GetFundDimensionValues(IEnumerable<IDesignation> allDesignations,
                                                IPlatformsFundDimension fundDimension) {
        var options = new List<string>();
        
        foreach (var designation in allDesignations) {
            if (fundDimension is PlatformsFundDimension1) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension1, x => x.Dimension1Options));
            } else if (fundDimension is PlatformsFundDimension2) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension2, x => x.Dimension2Options));
            } else if (fundDimension is PlatformsFundDimension3) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension3, x => x.Dimension3Options));
            } else if (fundDimension is PlatformsFundDimension4) {
                // No fund dimension 4
            } else {
                throw UnrecognisedValueException.For(fundDimension);
            }
        }

        return options;
    }
    
    private IReadOnlyList<string> GetAllowedOptions(IDesignation designation,
                                                    IFundDimensionValue fundDimensionValue,
                                                    Func<IFundDimensionsOptions, IEnumerable<IFundDimensionValue>> getFundDimensionValues) {
        if (fundDimensionValue.HasValue()) {
            return fundDimensionValue.Name.Yield().ToList();
        } else {
            if (designation is FundDesignation fundDesignation) {
                return getFundDimensionValues(fundDesignation.DonationItem).Select(x => x.Name).ToList();
            } else if (designation is SponsorshipDesignation sponsorshipDesignation) {
                return getFundDimensionValues(sponsorshipDesignation.Scheme).Select(x => x.Name).ToList();
            } else if (designation is FeedbackDesignation feedbackDesignation) {
                return getFundDimensionValues(feedbackDesignation.Scheme).Select(x => x.Name).ToList();
            } else {
                throw UnrecognisedValueException.For(designation);
            }
        }
    }
}