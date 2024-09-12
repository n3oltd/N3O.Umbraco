using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public abstract class AccountManager : IAccountManager {
    private readonly AccountCookie _accountCookie;

    public AccountManager(AccountCookie accountCookie) {
        _accountCookie = accountCookie;
    }

    public abstract Task CreateAccountAsync(AccountReq account);
    public abstract Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email);
    public abstract Task UpdateAccountAsync(AccountReq account);

    public Task SelectAccount(string accountId, string accountReference) {
        _accountCookie.Set(accountId, accountReference);
        
        return Task.CompletedTask;
    }
}