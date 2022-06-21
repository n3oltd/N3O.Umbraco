using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Handlers;

public class UpdateRegularGivingOptionsHandler :
    IRequestHandler<UpdateRegularGivingOptionsCommand, RegularGivingOptionsReq, CheckoutRes> {
    private readonly IRepository<Entities.Checkout> _repository;
    private readonly IUmbracoMapper _mapper;

    public UpdateRegularGivingOptionsHandler(IRepository<Entities.Checkout> repository, IUmbracoMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<CheckoutRes> Handle(UpdateRegularGivingOptionsCommand req, CancellationToken cancellationToken) {
        var checkout = await req.CheckoutRevisionId.RunAsync(_repository.GetAsync, true);

        checkout.UpdateRegularGivingOptions(req.Model);
        
        await _repository.UpdateAsync(checkout);
        
        var res = _mapper.Map<Entities.Checkout, CheckoutRes>(checkout);

        return res;
    }
}
