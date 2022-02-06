using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Payment, PaymentRes>((_, _) => new PaymentRes(), Map);
        }

        // Umbraco.Code.MapAll -Type -Method -Status -HasError -IsComplete -IsInProgress
        private void Map(Payment src, PaymentRes dest, MapperContext ctx) {
            dest.Card = ctx.Map<CardPayment, CardPaymentRes>(src.Card);
            dest.DeclinedReason = src.DeclinedReason;
            dest.IsDeclined = src.IsDeclined;
            dest.IsPaid = src.IsPaid;
        }
    }
}