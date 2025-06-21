using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

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
    
    private IEnumerable<PublishedDonateMenuEntry> GetTelethonEntries(MapperContext ctx,
                                                                     IEnumerable<CampaignContent> allCampaigns) {
        var campaigns = allCampaigns.Where(x => x.Type == CampaignTypes.Telethon).ToList();

        var menuEntries = new List<PublishedDonateMenuEntry>();
        
        foreach (var campaign in campaigns) {
            var menuEntry = new PublishedDonateMenuEntry();
            
            menuEntry.Type = DonateMenuEntryType.Campaign;
            menuEntry.Name = campaign.Name;
            menuEntry.Icon = campaign.Icon.GetPublishedUri();
            menuEntry.Campaign = ctx.Map<CampaignContent, PublishedCampaign>(campaign);
                
            menuEntries.Add(menuEntry);
        }

        return menuEntries;
    }

    private PublishedDonateMenuEntry GetStandardEntries(MapperContext ctx, IEnumerable<CampaignContent> allCampaigns) {
        var menuEntry = new PublishedDonateMenuEntry();
        
        menuEntry.Type = DonateMenuEntryType.CampaignsList;
        menuEntry.Name = "Campaigns"; // TODO
        menuEntry.Icon = null; // TODO
        
        menuEntry.CampaignsList = new PublishedDonateMenuCampaignsListEntry();
        menuEntry.CampaignsList.Campaigns = allCampaigns.Where(x => x.Type == CampaignTypes.Standard)
                                                        .Select(ctx.Map<CampaignContent, PublishedCampaignSummary>)
                                                        .ToList();
        
        return menuEntry;
    }

    private IEnumerable<PublishedDonateMenuEntry> GetBrowsableEntries(IEnumerable<CampaignContent> allCampaigns) {
        var allDesignations = allCampaigns.SelectMany(x => x.Designations).ToList();
        
        var menuEntries = new List<PublishedDonateMenuEntry>();
        
        var fundStructure = _contentLocator.Single<FundStructureContent>();

        if (fundStructure.FundDimension1?.Browsable == true) {
            menuEntries.Add(GetBrowsableEntry(allDesignations, fundStructure.FundDimension1, 1));
        }
        
        if (fundStructure.FundDimension2?.Browsable == true) {
            menuEntries.Add(GetBrowsableEntry(allDesignations, fundStructure.FundDimension2, 2));
        }
        
        if (fundStructure.FundDimension3?.Browsable == true) {
            menuEntries.Add(GetBrowsableEntry(allDesignations, fundStructure.FundDimension3, 3));
        }
        
        if (fundStructure.FundDimension4?.Browsable == true) {
            menuEntries.Add(GetBrowsableEntry(allDesignations, fundStructure.FundDimension4, 4));
        }
        
        return menuEntries;
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
    
    private List<string> GetFundDimensionValues<T>(IEnumerable<DesignationContent> allDesignations,
                                                   FundDimensionContent<T> fundDimension) {
        var values = new List<string>();
        
        foreach (var designation in allDesignations) {
            if (fundDimension.Number == 1) {
                values.AddRangeIfNotExists(GetAllowedOptions(designation.Dimension1,
                                                             designation.GetFundDimensionOptions().Dimension1Options));
            } else if (fundDimension.Number == 2) {
                values.AddRangeIfNotExists(GetAllowedOptions(designation.Dimension2,
                                                             designation.GetFundDimensionOptions().Dimension2Options));
            } else if (fundDimension.Number == 3) {
                values.AddRangeIfNotExists(GetAllowedOptions(designation.Dimension3,
                                                             designation.GetFundDimensionOptions().Dimension3Options));
            } else if (fundDimension.Number == 4) {
                values.AddRangeIfNotExists(GetAllowedOptions(designation.Dimension4,
                                                             designation.GetFundDimensionOptions().Dimension4Options));
            } else {
                throw UnrecognisedValueException.For(fundDimension.Number);
            }
        }

        return values.OrderBy(x => x).ToList();
    }
    
    private IEnumerable<string> GetAllowedOptions(IFundDimensionValue fundDimensionValue,
                                                  IEnumerable<IFundDimensionValue> allowedFundDimensionValues) {
        if (fundDimensionValue.HasValue()) {
            return fundDimensionValue.Name.Yield();
        } else {
            return allowedFundDimensionValues.Select(x => x.Name);
        }
    }
}