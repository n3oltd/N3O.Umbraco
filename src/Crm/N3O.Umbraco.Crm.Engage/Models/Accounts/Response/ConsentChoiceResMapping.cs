using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class ConsentChoiceResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public ConsentChoiceResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectPreferenceSelectionRes, ConsentChoiceRes>((_, _) => new ConsentChoiceRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectPreferenceSelectionRes src, ConsentChoiceRes dest, MapperContext ctx) {
        dest.Channel = src.Channel.IfNotNull(x => _lookups.FindByIdOrName<ConsentChannel>(x.ToString()));
        dest.Category = src.Category.IfNotNull(x => _lookups.FindByIdOrName<ConsentCategory>(x.ToString()));

        if (src.Preference == Preference.NoResponse) {
            dest.Response = ConsentResponses.NoResponse;
        } else if (src.Preference == Preference.OptIn) {
            dest.Response = ConsentResponses.OptIn;
        } else if (src.Preference == Preference.OptOut) {
            dest.Response = ConsentResponses.OptOut;
        } else {
            throw UnrecognisedValueException.For(src.Preference);
        } 
    }
}