namespace N3O.Umbraco.Payments.DirectDebitUK;

public interface IUKBankAccountValidatorFactory {
    IUKBankAccountValidator CreateValidator();
}