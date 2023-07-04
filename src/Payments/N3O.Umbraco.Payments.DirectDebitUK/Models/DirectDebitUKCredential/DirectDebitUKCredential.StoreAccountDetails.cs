namespace N3O.Umbraco.Payments.DirectDebitUK.Models;

public partial class DirectDebitUKCredential {
    public void StoreAccountDetails(IUKBankAccount bankAccount) {
        BankAccount = new UKBankAccount(bankAccount);
        
        SetUp();
    }
}