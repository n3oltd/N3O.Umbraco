// using Microsoft.Extensions.Logging;
// using N3O.Umbraco.Hosting;
// using N3O.Umbraco.Payments.PayPal.Models;
// using System;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Payments.PayPal.Controllers {
//     public class CheckoutPayPalSinglePaymentController : ApiController {
//         public CheckoutPayPalSinglePaymentController(ILogger logger) : base(logger) { }
//         
//         protected override Task<CheckoutStageRes> ExecuteAsync(PayPalPaymentReq req) {
//             _payPalPayments.CheckoutSinglePayment(Checkout, req.Email, req.TransactionId);
//
//             return Task.FromResult(Success());
//         }
//     }
// }