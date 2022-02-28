using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Payments.GoCardless.Models;

namespace N3O.Umbraco.Payments.GoCardless {
    public class GoCardlessDataEntryConfiguation : PaymentMethodDataEntryConfiguration<GoCardlessPaymentMethod, GoCardlessDataEntrySettings> {
        private readonly IHostEnvironment _environment;

        public GoCardlessDataEntryConfiguation(IHostEnvironment environment) {
            _environment = environment;
        }
        protected override GoCardlessDataEntrySettings GetConfig(GoCardlessPaymentMethod paymentMethod) {
            GoCardlessDataEntrySettings settings = null;
            if (_environment.IsProduction()) {
                settings = new GoCardlessDataEntrySettings("https://api.gocardless.com", "https://api.gocardless.com/flow/");
            } else {
                settings = new GoCardlessDataEntrySettings("https://api-sandbox..gocardless.com", "https://api-sandbox..gocardless.com/flow/");
            }

            return settings;
        }
    }
}