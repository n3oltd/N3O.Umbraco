using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public interface IPaymentsScope {
        Task DoAsync<TPaymentObject>(Func<IPaymentsFlow, TPaymentObject, Task> actionAsync,
                                     CancellationToken cancellationToken = default)
            where TPaymentObject : PaymentObject, new();

        Task<T> GetAsync<T>(Func<IPaymentsFlow, T> get, CancellationToken cancellationToken = default);
    }
}