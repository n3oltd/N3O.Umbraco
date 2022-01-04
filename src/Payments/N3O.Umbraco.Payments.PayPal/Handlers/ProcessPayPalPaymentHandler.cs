// using N3O.Umbraco.Entities;
// using N3O.Umbraco.Mediator;
// using N3O.Umbraco.Payments.PayPal.Commands;
// using N3O.Umbraco.Payments.PayPal.Entities;
// using N3O.Umbraco.Payments.PayPal.Models;
// using System;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Payments.PayPal.Handlers {
//     public class ProcessPayPalPaymentHandler : IRequestHandler<ProcessPayPalPaymentCommand, PayPalPaymentReq, Uri> {
//         private readonly IRepository<PayPalPayment> _repository;
//         private readonly IPaymentsFlow _paymentsFlow;
//
//         public ProcessPayPalPaymentHandler(IRepository<PayPalPayment> repository, IPaymentsFlow paymentsFlow) {
//             _repository = repository;
//             _paymentsFlow = paymentsFlow;
//         }
//         
//         public Task<Uri> Handle(ProcessPayPalPaymentCommand req, CancellationToken cancellationToken) {
//             throw new NotImplementedException();
//         }
//     }
// }