using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Locks;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.References;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Handlers {
    public class GetOrBeginCheckoutHandler : IRequestHandler<GetOrBeginCheckoutCommand, None, CheckoutRes> {
        private readonly ICheckoutIdAccessor _checkoutIdAccessor;
        private readonly IRepository<Entities.Checkout> _repository;
        private readonly ICartAccessor _cartAccessor;
        private readonly ICounters _counters;
        private readonly ILock _lock;
        private readonly IUmbracoMapper _mapper;

        public GetOrBeginCheckoutHandler(ICheckoutIdAccessor checkoutIdAccessor,
                                         IRepository<Entities.Checkout> repository,
                                         ICartAccessor cartAccessor,
                                         ICounters counters,
                                         ILock @lock,
                                         IUmbracoMapper mapper) {
            _checkoutIdAccessor = checkoutIdAccessor;
            _repository = repository;
            _cartAccessor = cartAccessor;
            _counters = counters;
            _lock = @lock;
            _mapper = mapper;
        }
        
        public async Task<CheckoutRes> Handle(GetOrBeginCheckoutCommand req, CancellationToken cancellationToken) {
            var checkout = await GetOrCreateAsync(cancellationToken);
            var res = _mapper.Map<Entities.Checkout, CheckoutRes>(checkout);

            return res;
        }
    
        private async Task<Entities.Checkout> GetOrCreateAsync(CancellationToken cancellationToken) {
            var checkoutId = _checkoutIdAccessor.GetCheckoutId();
            
            var result = await _lock.LockAsync(checkoutId.ToString(), async () => {
                var checkout = await _repository.GetAsync(checkoutId, cancellationToken);

                if (checkout == null) {
                    var cart = await _cartAccessor.GetAsync(cancellationToken);
                    
                    checkout = await Entities.Checkout.CreateAsync(_counters, checkoutId, cart);

                    await _repository.InsertAsync(checkout, cancellationToken);
                }

                return checkout;
            });

            return result;
        }
    }
}