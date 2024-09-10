using N3O.Umbraco.Crm.Context;
using System;

namespace N3O.Umbraco.Crm;

public class AccountInfoAccessor : IAccountInfoAccessor {
    private readonly Lazy<AccountCookie> _accountCookie;
    private string _accountId;
    private string _accountReference;

    public AccountInfoAccessor(Lazy<AccountCookie> accountCookie) {
        _accountCookie = accountCookie;
    }

    public string GetId() {
        if (_accountId == null) {
            _accountId = _accountCookie.Value.GetId();
        }

        return _accountId;
    }

    public string GetReference() {
        if (_accountReference == null) {
            _accountReference = _accountCookie.Value.GetReference();
        }

        return _accountReference;
    }
}
