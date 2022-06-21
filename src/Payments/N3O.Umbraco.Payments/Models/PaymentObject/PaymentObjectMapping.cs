using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models;

public class PaymentObjectMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PaymentObject, PaymentObjectRes>((_, _) => new PaymentObjectRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PaymentObject src, PaymentObjectRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Method = src.Method;
        dest.Status = src.Status;
        dest.ErrorMessage = src.ErrorMessage;
        dest.HasError = src.HasError;
        dest.IsComplete = src.IsComplete;
        dest.IsInProgress = src.IsInProgress;
    }
}
