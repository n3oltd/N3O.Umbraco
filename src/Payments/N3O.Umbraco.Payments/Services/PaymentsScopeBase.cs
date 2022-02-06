using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public abstract class PaymentsScopeBase : IPaymentsScope {
        public async Task<PaymentFlowRes<T>> DoAsync<T>(Func<IPaymentsFlow, T, Task> actionAsync,
                                                        CancellationToken cancellationToken = default)
            where T : PaymentObject, new() {
            var flow = await LoadAsync(cancellationToken);
            
            T paymentObject;

            if (typeof(T).IsSubclassOfType(typeof(Payment))) {
                paymentObject = GetOrCreate<T>(flow, PaymentObjectTypes.Payment);
            } else if (typeof(T).IsSubclassOfType(typeof(Credential))) {
                paymentObject = GetOrCreate<T>(flow, PaymentObjectTypes.Credential);
            } else {
                throw new NotImplementedException();
            }

            try {
                await actionAsync(flow, paymentObject);
            } catch (Exception ex) {
                paymentObject.UnhandledError(ex);
            }

            await UpdateAsync(flow, cancellationToken);

            return new PaymentFlowRes<T>(flow, paymentObject);
        }

        protected abstract Task<IPaymentsFlow> LoadAsync(CancellationToken cancellationToken);
        protected abstract Task UpdateAsync(IPaymentsFlow flow, CancellationToken cancellationToken);

        private T GetOrCreate<T>(IPaymentsFlow flow, PaymentObjectType paymentObjectType)
            where T : PaymentObject, new() {
            var existing = flow.GetPaymentObject(paymentObjectType);

            if (existing is T paymentObject) {
                return paymentObject;
            } else {
                if (existing?.Status == PaymentObjectStatuses.Complete) {
                    throw new Exception("Cannot change payment method at this time");
                }
                
                var newPaymentObject = new T();
                flow.SetPaymentObject(paymentObjectType, newPaymentObject);

                return newPaymentObject;
            }
        }
    }
}