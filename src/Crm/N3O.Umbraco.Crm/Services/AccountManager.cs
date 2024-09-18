using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Accounts.Exceptions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm;

public abstract class AccountManager : IAccountManager {
    private static readonly MemoryCache MemoryCache = new(new MemoryCacheOptions());

    private readonly IMemberManager _memberManager;
    private readonly AccountCookie _accountCookie;

    protected AccountManager(IMemberManager memberManager, AccountCookie accountCookie) {
        _memberManager = memberManager;
        _accountCookie = accountCookie;
    }
    
    public async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        var cacheKey = $"{nameof(FindAccountsByEmailAsync)}{email}";

        var accounts = await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry => {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

            return await FindAccountsWithEmailAsync(email);
        });

        return accounts;
    }

    public async Task SelectAccountAsync(string accountId, string accountReference, string accountToken) {
        var memberEmail = await _memberManager.GetCurrentPublishedMemberEmailAsync();

        if (!memberEmail.HasValue()) {
            throw new Exception("Could not find email of signed in member");
        }
        
        var allowedAccounts = await FindAccountsByEmailAsync(memberEmail);

        if (allowedAccounts.None(x => x.Id == accountId)) {
            throw new InvalidAccountException();
        }

        _accountCookie.Set(accountId, accountReference, accountToken);
    }

    public abstract Task CreateAccountAsync(AccountReq account);
    public abstract Task UpdateAccountAsync(AccountReq account);
    protected abstract Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email);
}