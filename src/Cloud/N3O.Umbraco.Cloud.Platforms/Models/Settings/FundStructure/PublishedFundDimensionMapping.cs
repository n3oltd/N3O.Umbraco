using N3O.Umbraco.Cloud.Platforms.Lookups;
using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using Umbraco.Cms.Core.Mapping;
using FundDimensionSelector = N3O.Umbraco.Cloud.Platforms.Lookups.FundDimensionSelector;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class FundDimensionMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public FundDimensionMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPlatformsFundDimension, UmbracoFundDimensionReq>((_, _) => new UmbracoFundDimensionReq(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(IPlatformsFundDimension src, UmbracoFundDimensionReq dest, MapperContext ctx) {
        dest.Browsable = src.Browsable;
        dest.Searchable = src.Searchable;
        
        dest.View = new PublishedFundDimensionView();
        dest.View.Selector = GetFundDimensionSelector(src.Selector);

        if (dest.View.Selector == Website.Connect.Clients.FundDimensionSelector.Toggle) {
            var toggleValue = src.ToggleValue.As<PlatformsFundDimensionToggle>();
            
            dest.View.Toggle = new PublishedFundDimensionToggleOptions();
        
            dest.View.Toggle.Label = toggleValue.Label;
            dest.View.Toggle.OffValue = toggleValue.OffValue;
            dest.View.Toggle.OnValue = toggleValue.OnValue;
        }
    }

    private Website.Connect.Clients.FundDimensionSelector GetFundDimensionSelector(FundDimensionSelector selector) {
        if (selector == FundDimensionSelectors.Dropdown) {
            return Website.Connect.Clients.FundDimensionSelector.Dropdown;
        } else if (selector == FundDimensionSelectors.Toggle) {
            return Website.Connect.Clients.FundDimensionSelector.Toggle;
        } else {
            throw UnrecognisedValueException.For(selector);
        }
    }
}