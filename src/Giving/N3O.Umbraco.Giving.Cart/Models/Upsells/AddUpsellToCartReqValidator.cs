using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Cart.Models; 

public class AddUpsellToCartReqValidator : ModelValidator<AddUpsellToCartReq> {
    public AddUpsellToCartReqValidator(IFormatter formatter) : base(formatter) {
        // No validation required
    }
}