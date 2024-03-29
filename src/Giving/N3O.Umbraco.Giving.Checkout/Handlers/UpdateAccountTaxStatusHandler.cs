﻿using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.TaxRelief;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Handlers;

public class UpdateAccountTaxStatusHandler :
    IRequestHandler<UpdateAccountTaxStatusCommand, TaxStatusReq, CheckoutRes> {
    private readonly IRepository<Entities.Checkout> _repository;
    private readonly IUmbracoMapper _mapper;
    private readonly IContentCache _contentCache;
    private readonly ITaxReliefSchemeAccessor _taxReliefSchemeAccessor;

    public UpdateAccountTaxStatusHandler(IRepository<Entities.Checkout> repository,
                                         IUmbracoMapper mapper,
                                         IContentCache contentCache,
                                         ITaxReliefSchemeAccessor taxReliefSchemeAccessor) {
        _repository = repository;
        _mapper = mapper;
        _contentCache = contentCache;
        _taxReliefSchemeAccessor = taxReliefSchemeAccessor;
    }
    
    public async Task<CheckoutRes> Handle(UpdateAccountTaxStatusCommand req, CancellationToken cancellationToken) {
        var checkout = await req.CheckoutRevisionId.RunAsync(_repository.GetAsync, true, cancellationToken);

        checkout.UpdateAccount(_contentCache,
                               _taxReliefSchemeAccessor,
                               account => account.WithUpdatedTaxStatus(req.Model.TaxStatus));
        
        await _repository.UpdateAsync(checkout);
        
        var res = _mapper.Map<Entities.Checkout, CheckoutRes>(checkout);

        return res;
    }
}
