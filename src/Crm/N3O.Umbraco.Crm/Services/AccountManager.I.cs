using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface IAccountManager {
    Task CreateAccountAsync(AccountReq account);
    Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email);
    Task SelectAccountAsync(AccountRes account);
    Task UpdateAccountAsync(AccountReq account);
}