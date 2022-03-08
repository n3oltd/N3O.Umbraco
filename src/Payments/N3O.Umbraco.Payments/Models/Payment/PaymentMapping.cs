using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Payment, PaymentRes>((_, _) => new PaymentRes(), Map);
        }

        // Umbraco.Code.MapAll -Type -Method -ErrorMessage -Status -HasError -IsComplete -IsInProgress
        private void Map(Payment src, PaymentRes dest, MapperContext ctx) {
            ctx.Map<PaymentObject, PaymentObjectRes>(src, dest);
            
            dest.Card = src.Card.IfNotNull(ctx.Map<CardPayment, CardPaymentRes>);
            dest.DeclinedReason = src.DeclinedReason;
            dest.IsDeclined = src.IsDeclined;
            dest.IsPaid = src.IsPaid;
        }
    }
}