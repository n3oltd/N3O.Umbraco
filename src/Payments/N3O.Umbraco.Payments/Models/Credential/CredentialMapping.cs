using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models {
    public class CredentialMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Credential, CredentialRes>((_, _) => new CredentialRes(), Map);
        }

        // Umbraco.Code.MapAll -Type -Method -Status -HasError -IsComplete -IsInProgress
        private void Map(Credential src, CredentialRes dest, MapperContext ctx) {
            dest.AdvancePayment = ctx.Map<Payment, PaymentRes>(src.AdvancePayment);
            dest.IsSetUp = src.IsSetUp;
        }
    }
}