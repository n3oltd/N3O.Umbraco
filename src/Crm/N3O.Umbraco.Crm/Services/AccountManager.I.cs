using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface IAccountManager {
    Task CreateAccountAsync(AccountReq account);
    Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email);
    Task SelectAccount(string accountId, string accountReference);
    Task UpdateAccountAsync(AccountReq account);
}