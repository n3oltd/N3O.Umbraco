using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public interface IPaymentsFlow : IEntity, IBillingInfoAccessor {
        IEnumerable<PaymentObject> PaymentObjects { get; }
        string NextUrl { get; }

        void AddOrReplacePaymentObject<TPaymentObject>(TPaymentObject paymentObject) where TPaymentObject : PaymentObject, new();
        Task<Uri> OnCompleteAsync(PaymentObject paymentObject, CancellationToken cancellationToken);
        Task<Uri> OnErrorAsync(PaymentObject paymentObject, Exception exception, CancellationToken cancellationToken);
    }
}