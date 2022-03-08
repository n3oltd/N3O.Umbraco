using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Handlers {
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountReq, CheckoutRes> {
        private readonly IRepository<Entities.Checkout> _repository;
        private readonly IUmbracoMapper _mapper;

        public UpdateAccountHandler(IRepository<Entities.Checkout> repository, IUmbracoMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<CheckoutRes> Handle(UpdateAccountCommand req, CancellationToken cancellationToken) {
            var checkout = await req.CheckoutRevisionId.RunAsync(_repository.GetAsync, true);

            checkout.UpdateAccount(req.Model);
            
            await _repository.UpdateAsync(checkout);
            
            var res = _mapper.Map<Entities.Checkout, CheckoutRes>(checkout);

            return res;
        }
    }
}