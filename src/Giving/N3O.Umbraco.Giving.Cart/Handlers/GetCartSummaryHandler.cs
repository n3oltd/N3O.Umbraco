using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers {
    public class GetCartSummaryHandler : IRequestHandler<GetCartSummaryQuery, None, CartSummaryRes> {
        private readonly ICartAccessor _cartAccessor;

        public GetCartSummaryHandler(ICartAccessor cartAccessor) {
            _cartAccessor = cartAccessor;
        }
    
        public async Task<CartSummaryRes> Handle(GetCartSummaryQuery req, CancellationToken cancellationToken) {
            var cart = await _cartAccessor.GetAsync(cancellationToken);

            var summary = new CartSummaryRes();
            summary.RevisionId = cart.RevisionId;
            summary.ItemCount = cart.Donation.Allocations.Count() + cart.RegularGiving.Allocations.Count();

            return summary;
        }
    }
}