using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using DesignationType = N3O.Umbraco.Cloud.Platforms.Lookups.DesignationType;

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

        var telethonEntries = GetTelethonEntries(ctx, src.Campaigns);
        var standardEntries = GetStandardEntries(ctx, src.Campaigns);
        var browsableEntries = GetBrowsableEntries(src.Campaigns);

        entries.AddRange(telethonEntries);
        entries.Add(standardEntries);
        entries.AddRange(browsableEntries);
        
        dest.Entries = entries;
    }

    private PublishedDonateMenuEntry GetStandardEntries(MapperContext ctx, IEnumerable<CampaignContent> campaigns) {
        var standardCampaigns = campaigns.Where(x => x.Type == CampaignTypes.Standard);
        
        var standardEntry = new PublishedDonateMenuEntry();
        standardEntry.Type = DonateMenuEntryType.CampaignsList;
        standardEntry.Name = "Campaigns";
        standardEntry.Icon = null; // TODO
        standardEntry.CampaignsList = new PublishedDonateMenuCampaignsListEntry();
        standardEntry.CampaignsList.Campaigns = standardCampaigns.Select(ctx.Map<CampaignContent, PublishedCampaignSummary>).ToList();
        
        return standardEntry;
    }

    private IEnumerable<PublishedDonateMenuEntry> GetTelethonEntries(MapperContext ctx, IEnumerable<CampaignContent> campaigns) {
        var telethonCampaigns = campaigns.Where(x => x.Type == CampaignTypes.Telethon);

        var telethonCampaignsPub = new List<PublishedDonateMenuEntry>();
        
        foreach (var telethonCampaign in telethonCampaigns) {
            var telethonEntry = new PublishedDonateMenuEntry();
            telethonEntry.Type = DonateMenuEntryType.Campaign;
            telethonEntry.Name = telethonCampaign.Name;
            telethonEntry.Icon = new Uri(telethonCampaign.Image.GetCropUrl(urlMode: UrlMode.Absolute));
            telethonEntry.Campaign = ctx.Map<CampaignContent, PublishedCampaign>(telethonCampaign);
                
            telethonCampaignsPub.Add(telethonEntry);
        }

        return telethonCampaignsPub;
    }

    private IEnumerable<PublishedDonateMenuEntry> GetBrowsableEntries(IEnumerable<CampaignContent> allCampaigns) {
        var allDesignations = allCampaigns.SelectMany(x => x.Designations).ToList();
        
        var entries = new List<PublishedDonateMenuEntry>();
        
        var fundDimensions = _contentLocator.Single<FundStructureContent>();

        if (fundDimensions.FundDimension1?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimensions.FundDimension1, 1));
        }
        
        if (fundDimensions.FundDimension2?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimensions.FundDimension2, 2));
        }
        
        if (fundDimensions.FundDimension3?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimensions.FundDimension3, 3));
        }
        
        if (fundDimensions.FundDimension4?.Browsable == true) {
            entries.Add(GetBrowsableEntry(allDesignations, fundDimensions.FundDimension4, 4));
        }
        
        return entries;
    }

    private PublishedDonateMenuEntry GetBrowsableEntry<T>(IEnumerable<DesignationContent> allDesignations,
                                                          FundDimensionContent<T> fundDimension,
                                                          int dimensionNumber) {
        var entry = new PublishedDonateMenuEntry();
        entry.Type = DonateMenuEntryType.FundDimensionSearch;
        entry.Name = fundDimension.Content().Name;
        entry.Icon = null; //TODO
        
        entry.FundDimensionSearch = new PublishedDonateMenuFundDimensionSearchEntry();
        entry.FundDimensionSearch.DimensionNumber = dimensionNumber;
        entry.FundDimensionSearch.Values = GetFundDimensionValues(allDesignations, fundDimension);
        
        return entry;
    }
    
    private List<string> GetFundDimensionValues<T>(IEnumerable<DesignationContent> allDesignations, FundDimensionContent<T> fundDimension) {
        var options = new List<string>();
        
        foreach (var designation in allDesignations) {
            if (fundDimension is FundDimension1Content) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension1, designation.GetFundDimensionOptions().Dimension1Options));
            } else if (fundDimension is FundDimension2Content) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension2, designation.GetFundDimensionOptions().Dimension2Options));
            } else if (fundDimension is FundDimension3Content) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension3, designation.GetFundDimensionOptions().Dimension3Options));
            } else if (fundDimension is FundDimension4Content) {
                options.AddRangeIfNotExists(GetAllowedOptions(designation, designation.Dimension4, designation.GetFundDimensionOptions().Dimension4Options));
            } else {
                throw UnrecognisedValueException.For(fundDimension);
            }
        }

        return options;
    }
    
    private IReadOnlyList<string> GetAllowedOptions(DesignationContent designation,
                                                    IFundDimensionValue fundDimensionValue,
                                                    IEnumerable<IFundDimensionValue> allowedFundDimensionValues) {
        if (fundDimensionValue.HasValue()) {
            return fundDimensionValue.Name.Yield().ToList();
        } else {
            return allowedFundDimensionValues.Select(x => x.Name).ToList();
        }
    }
}