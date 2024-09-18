using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm;

public abstract class AccountManager : IAccountManager {
    public static readonly MemoryCache MemoryCache = new MemoryCache(new MemoryCacheOptions());

    private readonly IMemberManager _memberManager;
    private readonly AccountCookie _accountCookie;

    protected AccountManager(IMemberManager memberManager, AccountCookie accountCookie) {
        _memberManager = memberManager;
        _accountCookie = accountCookie;
    }

    public abstract Task CreateAccountAsync(AccountReq account);

    public async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        var cacheKey = $"{nameof(FindAccountsByEmailAsync)}{email}";

        var accounts = await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry => {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

            return await FindAccountsWithEmailAsync(email);
        });

        return accounts;
    }

    public abstract Task UpdateAccountAsync(AccountReq account);

    public async Task SelectAccountAsync(string accountId, string accountReference, string accountToken) {
        var memberEmail = await _memberManager.GetCurrentPublishedMemberEmailAsync();
        var allowedAccounts = await FindAccountsByEmailAsync(memberEmail);

        if (allowedAccounts.None(x => x.Id == accountId)) {
            throw new InvalidAccountException("The account is not associated with the logged in member");
        }

        _accountCookie.Set(accountId, accountReference, accountToken);
    }

    public abstract Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email);
}