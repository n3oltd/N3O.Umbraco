using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Crm;

public class AccountIdAccessor : IAccountIdAccessor {
    private readonly Lazy<AccountCookie> _accountCookie;
    private string _accountId;

    public AccountIdAccessor(Lazy<AccountCookie> accountCookie) {
        _accountCookie = accountCookie;
    }

    public string GetId() {
        if (_accountId == null) {
            _accountId = GetFromCookie();

            if (_accountId == null) {
                _accountCookie.Value.Reset();
                
                _accountId = GetFromCookie();
            }
        }

        return _accountId;
    }

    private RevisionId GetFromCookie() {
        return _accountCookie.Value.GetValue();
    }
}
