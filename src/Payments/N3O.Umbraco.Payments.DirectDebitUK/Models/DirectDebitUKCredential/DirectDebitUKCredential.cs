using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.DirectDebitUK.Models;

public partial class DirectDebitUKCredential : Credential {
    public override PaymentMethod Method => DirectDebitUKConstants.PaymentMethod;

    public UKBankAccount BankAccount { get; private set; }
}