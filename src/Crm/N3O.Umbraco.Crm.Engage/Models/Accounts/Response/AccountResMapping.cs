using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;
using Umbraco.Cms.Core.Mapping;
using AccountType = N3O.Umbraco.Accounts.Lookups.AccountType;

namespace N3O.Umbraco.Crm.Models;

public class AccountResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public AccountResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectAccountRes, AccountRes>((_, _) => new AccountRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectAccountRes src, AccountRes dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Reference = src.Reference;
        dest.Type = _lookups.FindByIdOrName<AccountType>(src.Type?.ToString());
        dest.Individual = src.Individual.IfNotNull(ctx.Map<ConnectIndividualRes, IndividualRes>);
        dest.Organization = src.Organization.IfNotNull(ctx.Map<ConnectOrganizationRes, OrganizationRes>);
        dest.Address = src.Address.IfNotNull(ctx.Map<ConnectAddressRes, AddressRes>);
        dest.Email = src.Email.IfNotNull(ctx.Map<ConnectEmailRes, EmailRes>);
        dest.Telephone = src.Telephone.IfNotNull(ctx.Map<ConnectTelephoneRes, TelephoneRes>);
        dest.Consent = src.Preferences.IfNotNull(ctx.Map<ConnectPreferencesRes, ConsentRes>);
        dest.TaxStatus = src.TaxStatus.IfNotNull(ctx.Map<ConnectTaxStatusRes, TaxStatus>);
        dest.Token = src.Token;
    }
}