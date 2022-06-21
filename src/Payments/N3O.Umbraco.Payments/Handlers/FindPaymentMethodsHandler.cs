using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Criteria;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Queries;
using N3O.Umbraco.Payments.QueryFilters;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Handlers;

public class FindPaymentMethodsHandler :
    IRequestHandler<FindPaymentMethodsQuery, PaymentMethodCriteria, IEnumerable<PaymentMethodRes>> {
    private readonly ILookups _lookups;
    private readonly PaymentMethodsQueryFilter _queryFilter;
    private readonly IUmbracoMapper _mapper;

    public FindPaymentMethodsHandler(ILookups lookups,
                                     PaymentMethodsQueryFilter queryFilter,
                                     IUmbracoMapper mapper) {
        _lookups = lookups;
        _queryFilter = queryFilter;
        _mapper = mapper;
    }
    
    public Task<IEnumerable<PaymentMethodRes>> Handle(FindPaymentMethodsQuery req,
                                                      CancellationToken cancellationToken) {
        var allPaymentMethods = _lookups.GetAll<PaymentMethod>();
        var paymentMethods = _queryFilter.Apply(allPaymentMethods, req.Model);
        var res = paymentMethods.Select(_mapper.Map<PaymentMethod, PaymentMethodRes>).ToList();

        return Task.FromResult<IEnumerable<PaymentMethodRes>>(res);
    }
}
