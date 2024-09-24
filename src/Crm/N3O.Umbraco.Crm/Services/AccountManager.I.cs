using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface IAccountManager {
    Task<string> CreateAccountAsync(AccountReq account);
    Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email);
    Task UpdateAccountAsync(AccountReq account);
}