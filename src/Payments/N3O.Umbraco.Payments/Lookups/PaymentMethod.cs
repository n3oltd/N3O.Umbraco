using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Lookups {
    public abstract class PaymentMethod :  NamedLookup {
        private readonly Type _paymentObjectType;
        private readonly Type _credentialObjectType;

        protected PaymentMethod(string id, string name, Type paymentObjectType, Type credentialObjectType)
            : base(id, name) {
            _paymentObjectType = paymentObjectType;
            _credentialObjectType = credentialObjectType;
        }

        public Type GetType(PaymentObjectType objectType) {
            if (objectType == PaymentObjectTypes.Credential) {
                return _credentialObjectType;
            } else if (objectType == PaymentObjectTypes.Payment) {
                return _paymentObjectType;
            } else {
                throw UnrecognisedValueException.For(objectType);
            }
        }

        public virtual bool IsAvailable(Country country, Currency currency) {
            return true;
        }
    }

    public class PaymentMethods : LookupsCollection<PaymentMethod> {
        private static readonly IReadOnlyList<PaymentMethod> All;

        static PaymentMethods() {
            All = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                              t.IsSubclassOfType(typeof(PaymentMethod)) &&
                                              t.HasParameterlessConstructor())
                               .Select(t => (PaymentMethod) Activator.CreateInstance(t))
                               .ToList();
        }

        public override Task<IReadOnlyList<PaymentMethod>> GetAllAsync() {
            return Task.FromResult(All);
        }
    }
}