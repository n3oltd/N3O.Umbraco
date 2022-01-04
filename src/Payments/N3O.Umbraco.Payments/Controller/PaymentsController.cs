using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Controller {
    public abstract class PaymentsController : ApiController {
        private readonly Lazy<IPaymentsScope> _paymentsScope;
        
        protected PaymentsController(Lazy<IPaymentsScope> paymentsScope) {
            _paymentsScope = paymentsScope;
        }
        
        protected async Task<RedirectResult> NextAsync() {
            var url = await _paymentsScope.Value.GetAsync(flow => flow.NextUrl);

            return Redirect(url);
        }
    }
}