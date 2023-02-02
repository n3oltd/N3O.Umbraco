using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Giving.Cart.Entities;
using N3O.Umbraco.Mediator;
using Read.Core.Commands;
using Read.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Read.Core.Handlers; 

public class AddUpsellItemToCartHandler : IRequestHandler<AddUpsellItemToCartCommand, AddUpsellToCartReq, RevisionId> {
    private readonly IRepository<Cart> _repository;
    private readonly UpsellService _upsellHelper;
    
    public AddUpsellItemToCartHandler(IRepository<Cart> repository, UpsellService upsellHelper) {
        _repository = repository;
        _upsellHelper = upsellHelper;
    }

    public async Task<RevisionId> Handle(AddUpsellItemToCartCommand request, CancellationToken cancellationToken) {
        if (await _upsellHelper.IsUpsellItemAdded()) {
            throw new Exception("Upsell Item has already been added to the basket");
        }
        
        var cart = await _upsellHelper.AddUpsellItem(Guid.Parse(request.Model.UpsellItemId));
        
        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}