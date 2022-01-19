using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingOptionsReqValidator : ModelValidator<RegularGivingOptionsReq> {
        public RegularGivingOptionsReqValidator(IFormatter formatter) : base(formatter) {
            // No validation required
        }
    }
}
