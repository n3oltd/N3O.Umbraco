using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Accounts.Exceptions;
using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm;

public abstract class AccountManager : IAccountManager {
    private static readonly MemoryCache MemoryCache = new(new MemoryCacheOptions());

    private readonly IMemberManager _memberManager;
    private readonly AccountCookie _accountCookie;
    private readonly IFormatter _formatter;

    protected AccountManager(IMemberManager memberManager,
                             AccountCookie accountCookie,
                             IFormatter formatter) {
        _memberManager = memberManager;
        _accountCookie = accountCookie;
        _formatter = formatter;
    }

    public async Task<string> CreateAccountAsync(AccountReq account) {
        await VerifyEmailMatchesMemberAsync(account.Email.Address);

        var id = await CreateNewAccountAsync(account);

        await SelectAccountAsync(id, null, null);

        return id;
    }

    public async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        var cacheKey = $"{nameof(FindAccountsByEmailAsync)}{email}";

        var accounts = await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry => {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

            return await FindAccountsWithEmailAsync(email);
        });

        return accounts;
    }

    public async Task UpdateAccountAsync(AccountReq account) {
        await VerifyAccountAccessAsync(account.Id);
        await VerifyEmailMatchesMemberAsync(account.Email.Address);
        
        await UpdateExistingAccountAsync(account);
        
        await SelectAccountAsync(account.Id, account.Reference, account.GetToken(_formatter));
    }
    
    private async Task SelectAccountAsync(string accountId, string accountReference, string accountToken) {
        await VerifyAccountAccessAsync(accountId);
        
        _accountCookie.Set(accountId, accountReference, accountToken);
    }

    private async Task VerifyAccountAccessAsync(string accountId) {
        var memberEmail = await GetMemberEmailAsync();
        var allowedAccounts = await FindAccountsByEmailAsync(memberEmail);
        
        if (allowedAccounts.None(x => x.Id == accountId)) {
            throw new InvalidAccountException();
        }
    }
    
    private async Task VerifyEmailMatchesMemberAsync(string emailAddress) {
        var memberEmail = await GetMemberEmailAsync();

        if (!memberEmail.EqualsInvariant(emailAddress)) {
            throw new Exception("Cannot change email address associated with account");
        }
    }

    private async Task<string> GetMemberEmailAsync() {
        var memberEmail = await _memberManager.GetCurrentPublishedMemberEmailAsync();

        if (!memberEmail.HasValue()) {
            throw new Exception("Could not find email of signed in member");
        }

        return memberEmail;
    }

    protected abstract Task<string> CreateNewAccountAsync(AccountReq account);
    protected abstract Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email);
    protected abstract Task UpdateExistingAccountAsync(AccountReq account);
}