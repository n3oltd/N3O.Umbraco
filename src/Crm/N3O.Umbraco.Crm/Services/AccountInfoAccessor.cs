using N3O.Umbraco.Crm.Context;

namespace N3O.Umbraco.Crm;

public class AccountInfoAccessor : IAccountInfoAccessor {
    private readonly AccountCookie _accountCookie;
    private string _accountId;
    private string _accountReference;

    public AccountInfoAccessor(AccountCookie accountCookie) {
        _accountCookie = accountCookie;
    }

    public string GetId() {
        if (_accountId == null) {
            _accountId = _accountCookie.GetId();
        }

        return _accountId;
    }

    public string GetReference() {
        if (_accountReference == null) {
            _accountReference = _accountCookie.GetReference();
        }

        return _accountReference;
    }
}
