using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Commands;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Handlers;

public class SetCurrencyHandler : IRequestHandler<SetCurrencyCommand, None, CurrencyRes> {
    private readonly CurrencyCookie _currencyCookie;
    private readonly ILookups _lookups;
    private readonly IUmbracoMapper _mapper;

    public SetCurrencyHandler(CurrencyCookie currencyCookie, ILookups lookups, IUmbracoMapper mapper) {
        _currencyCookie = currencyCookie;
        _lookups = lookups;
        _mapper = mapper;
    }

    public Task<CurrencyRes> Handle(SetCurrencyCommand req, CancellationToken cancellationToken) {
        var currency = req.CurrencyCode.Run(_lookups.FindById<Currency>, true);

        if (!currency.Code.EqualsInvariant(_currencyCookie.GetValue())) {
            _currencyCookie.SetValue(currency.Code);
        }

        var res = _mapper.Map<Currency, CurrencyRes>(currency);

        return Task.FromResult(res);
    }
}
