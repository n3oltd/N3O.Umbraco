using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.References;
using System;

namespace N3O.Umbraco.Payments.Testing {
    public class TestPaymentsFlow : Entity, IPaymentsFlow {
        private readonly ILookups _lookups;

        public TestPaymentsFlow(ILookups lookups) {
            _lookups = lookups;
            Reference = new Reference(new ReferenceType("PM", 10000), 10012);
        }
        
        public Credential Credential { get; private set; }
        public Payment Payment { get; private set; }
        public Reference Reference { get; }

        public BillingInfo GetBillingInfo() {
            var unitedKingdom = _lookups.FindById<Country>("unitedKingdom");
            var title = _lookups.FindById<Title>("mr");
            var address = new Address("10 Downing Street", null, null, "London", null, "SW1A 2AB", unitedKingdom);
            var name = new Name(title, "Joe", "Bloggs");
            var email = new Email("joe.bloggs@n3o.ltd");
            var telphone = new Telephone(unitedKingdom, "0333 016 5130");
            
            return new BillingInfo(address, email,name, telphone);
        }

        public T GetOrCreatePaymentObject<T>() where T : PaymentObject, new() {
            if (typeof(T).IsSubclassOfType(typeof(Payment))) {
                if (Payment is T typedPayment) {
                    return typedPayment;
                } else {
                    Payment = new T() as Payment;
                    
                    return Payment as T;
                }
            } else if (typeof(T).IsSubclassOfType(typeof(Credential))) {
                if (Credential is T typedPayment) {
                    return typedPayment;
                } else {
                    Credential = new T() as Credential;
                    
                    return Credential as T;
                }
            } else {
                throw new NotImplementedException();
            }
        }
    }
}