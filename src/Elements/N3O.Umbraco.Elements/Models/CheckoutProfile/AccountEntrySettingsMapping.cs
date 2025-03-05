using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class AccountEntrySettingsMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    private readonly ILookups _lookups;
    private readonly IFormatter _formatter;

    public AccountEntrySettingsMapping(IContentLocator contentLocator, ILookups lookups, IFormatter formatter) {
        _contentLocator = contentLocator;
        _lookups = lookups;
        _formatter = formatter;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DataEntrySettingsContent, AccountEntrySettings>((_, _) => new AccountEntrySettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DataEntrySettingsContent src, AccountEntrySettings dest, MapperContext ctx) {
        var consentOptionsContent = _contentLocator.All<ConsentOptionContent>();
        var taxRelief = _contentLocator.Single<TaxReliefSettingsContent>();

        var settings = src.IfNotNull(x => x.ToDataEntrySettings(_lookups, consentOptionsContent));
        
        dest.Name = ctx.Map<NameDataEntrySettings, NameEntrySettings>(settings.Name);
        dest.Address = ctx.Map<AddressDataEntrySettings, AddressEntrySettings>(settings.Address);
        dest.Email = ctx.Map<EmailDataEntrySettings, EmailEntrySettings>(settings.Email);
        dest.Telephone = ctx.Map<PhoneDataEntrySettings, TelephoneEntrySettings>(settings.Phone);
        dest.TaxRelief = ctx.Map<TaxReliefSettingsContent, TaxReliefSettings>(taxRelief);
        
        // TODO need to map all properties otherwise build errors
        dest.Preferences = default;
    }
}