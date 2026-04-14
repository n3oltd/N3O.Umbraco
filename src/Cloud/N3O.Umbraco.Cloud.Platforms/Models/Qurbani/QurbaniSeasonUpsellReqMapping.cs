using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Context;
using N3O.Umbraco.Media;
using Umbraco.Cms.Core.Mapping;
using Money = N3O.Umbraco.Financial.Money;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonUpsellReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;

    public QurbaniSeasonUpsellReqMapping(IMediaUrl mediaUrl, IBaseCurrencyAccessor baseCurrencyAccessor) {
        _mediaUrl = mediaUrl;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonUpsellContent, QurbaniSeasonUpsellReq>((_, _) => new QurbaniSeasonUpsellReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonUpsellContent src, QurbaniSeasonUpsellReq dest, MapperContext ctx) {
        var baseCurrency = _baseCurrencyAccessor.GetBaseCurrency();
        var value = new Money(src.Amount, baseCurrency);
        
        dest.Name = src.Name;
        dest.Summary = src.Summary;
        dest.Icon = new SvgContentReq();
        dest.Icon = src.Icon.ToSvgContentReq(_mediaUrl);
        dest.FormState = src.ToDonationFormStateReq(value);
    }
}