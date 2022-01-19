using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models {
    public class AccountMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Account, AccountRes>((_, _) => new AccountRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Account src, AccountRes dest, MapperContext ctx) {
            dest.Name = ctx.Map<Name, NameRes>(src.Name);
            dest.Address = ctx.Map<Address, AddressRes>(src.Address);
            dest.Email = ctx.Map<Email, EmailRes>(src.Email);
            dest.Telephone = ctx.Map<Telephone, TelephoneRes>(src.Telephone);
            dest.TaxStatus = src.TaxStatus;
        }
    }
}