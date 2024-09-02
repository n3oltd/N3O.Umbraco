using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm.Engage;

// TODO SHagufta, implement this
public class EngageAccountManager : AccountManager {
    private readonly AccountsClient _accountsClient;

    public EngageAccountManager(AccountsClient accountsClient) {
        _accountsClient = accountsClient;
    }
    
    public override async Task CreateAccountAsync(AccountReq account) {
        // Forward the request to Engage, only bit that needs some thought is how we bubble up errors via
        // the validation/problem details mechanism
        throw new System.NotImplementedException();
    }

    public override async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        throw new System.NotImplementedException();
    }

    public override async Task UpdateAccountAsync(AccountReq account) {
        throw new System.NotImplementedException();
    }
}