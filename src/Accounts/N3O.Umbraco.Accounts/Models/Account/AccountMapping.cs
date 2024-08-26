using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class AccountMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Account, AccountRes>((_, _) => new AccountRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Account src, AccountRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Individual = ctx.Map<Individual, IndividualRes>(src.Individual);
        dest.Organization = ctx.Map<Organization, OrganizationRes>(src.Organization);
        dest.Address = ctx.Map<Address, AddressRes>(src.Address);
        dest.Email = ctx.Map<Email, EmailRes>(src.Email);
        dest.Telephone = ctx.Map<Telephone, TelephoneRes>(src.Telephone);
        dest.Consent = ctx.Map<Consent, ConsentRes>(src.Consent);
        dest.TaxStatus = src.TaxStatus;
    }
}