using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public interface IPaymentsScope {
        Task<PaymentFlowRes<T>> DoAsync<T>(Func<IPaymentsFlow, T, Task> actionAsync,
                                           CancellationToken cancellationToken = default)
            where T : PaymentObject, new();
    }
}