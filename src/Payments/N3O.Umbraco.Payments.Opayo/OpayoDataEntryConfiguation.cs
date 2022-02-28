using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo {
    public class OpayoDataEntryConfiguation : PaymentMethodDataEntryConfiguration<OpayoPaymentMethod, OpayoDataEntrySettings> {
        private readonly IHostEnvironment _environment;
        private readonly IContentCache _contentCache;

        public OpayoDataEntryConfiguation(IHostEnvironment environment, IContentCache contentCache) {
            _environment = environment;
            _contentCache = contentCache;
        }
        
        protected override OpayoDataEntrySettings GetConfig(OpayoPaymentMethod paymentMethod) {
            var settings = _contentCache.GetOpayoApiSettings(_environment);

            return new OpayoDataEntrySettings(settings.BaseUrl, settings.IntegrationKey);
        }
    }
}