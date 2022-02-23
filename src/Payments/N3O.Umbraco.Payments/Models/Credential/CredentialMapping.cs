using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Models {
    public class CredentialMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Credential, CredentialRes>((_, _) => new CredentialRes(), Map);
        }

        // Umbraco.Code.MapAll -Type -Method -Status -HasError -IsComplete -IsInProgress
        private void Map(Credential src, CredentialRes dest, MapperContext ctx) {
            ctx.Map<PaymentObject, PaymentObjectRes>(src, dest);

            dest.AdvancePayment = src.AdvancePayment.IfNotNull(ctx.Map<Payment, PaymentRes>);
            dest.SetupAt = src.SetupAt;
            dest.IsSetUp = src.IsSetUp;
        }
    }
}