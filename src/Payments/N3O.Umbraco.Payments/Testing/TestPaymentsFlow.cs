using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.References;

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
            var title = "Mr";
            var address = new Address("10 Downing Street", null, null, "London", null, "SW1A 2AB", unitedKingdom);
            var name = new Name(title, "Joe", "Bloggs");
            var email = new Email("joe.bloggs@n3o.ltd");
            var telephone = new Telephone(unitedKingdom, "0333 016 5130");
            
            return new BillingInfo(address, email,name, telephone);
        }

        public PaymentObject GetPaymentObject(PaymentObjectType type) {
            if (type == PaymentObjectTypes.Payment) {
                return Payment;
            } else if (type == PaymentObjectTypes.Credential) {
                return Credential;
            } else {
                throw UnrecognisedValueException.For(type);
            }
        }

        public void SetPaymentObject(PaymentObjectType type, PaymentObject paymentObject) {
            if (type == PaymentObjectTypes.Payment) {
                Payment = paymentObject as Payment;
            } else if (type == PaymentObjectTypes.Credential) {
                Credential = paymentObject as Credential;
            } else {
                throw UnrecognisedValueException.For(type);
            }
        }
    }
}