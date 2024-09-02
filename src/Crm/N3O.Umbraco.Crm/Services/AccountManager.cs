using N3O.Umbraco.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public abstract class AccountManager : IAccountManager {
    public abstract Task CreateAccountAsync(AccountReq account);
    public abstract Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email);
    public abstract Task UpdateAccountAsync(AccountReq account);
    
    public async Task SelectAccountAsync(AccountRes account) {
        // TODO Shagufta, use the account cookie to set the ID of the selected account
        throw new NotImplementedException();
    }
}