using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Models;

namespace N3O.Umbraco.Crm;

public class AccountIdentityAccessor : IAccountIdentityAccessor {
    private readonly AccountCookie _accountCookie;
    private string _accountId;
    private string _accountReference;
    private string _accountToken;

    public AccountIdentityAccessor(AccountCookie accountCookie) {
        _accountCookie = accountCookie;
    }

    public AccountIdentity Get() {
        return new AccountIdentity(GetId(), GetReference(), GetToken());
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
    
    public string GetToken() {
        if (_accountToken == null) {
            _accountToken = _accountCookie.GetToken();
        }

        return _accountToken;
    }
}
