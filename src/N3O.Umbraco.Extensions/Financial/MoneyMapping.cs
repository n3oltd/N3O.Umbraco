using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Financial {
    public class MoneyMapping : IMapDefinition {
        private readonly IFormatter _formatter;

        public MoneyMapping(IFormatter formatter) {
            _formatter = formatter;
        }
    
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Money, MoneyRes>((src, dest, _) => {
                dest.Amount = src.Amount;
                dest.Currency = src.Currency;
                dest.Text = _formatter.Number.FormatMoney(src);
            });
        }
    }
}