using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Accounts.Exceptions;
using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Cloud.Engage.Context;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Cloud.Exceptions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Engage;

public class AccountManager : IAccountManager {
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);
    private static readonly MemoryCache MemoryCache = new(new MemoryCacheOptions());

    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly AccountCookie _accountCookie;
    private readonly IFormatter _formatter;
    private readonly ClientFactory<AccountsClient> _clientFactory;
    private readonly IUmbracoMapper _mapper;
    private CloudApiClient<AccountsClient> _client;

    public AccountManager(IMemberManager memberManager,
                          IMemberService memberService,
                          AccountCookie accountCookie,
                          IFormatter formatter,
                          ClientFactory<AccountsClient> clientFactory,
                          IUmbracoMapper mapper) {
        _memberManager = memberManager;
        _memberService = memberService;
        _accountCookie = accountCookie;
        _formatter = formatter;
        _clientFactory = clientFactory;
        _mapper = mapper;
    }

    public async Task<string> CreateAccountAsync(AccountReq account) {
        await VerifyEmailMatchesMemberAsync(account.Email.Address);

        var id = await CreateNewAccountAsync(account);

        await SelectAccountAsync(id, null, null, null, null, null);

        return id;
    }

    public async Task<CreatedStatus<AccountRes>> CheckCreatedStatusAsync(string accountId) {
        var res = await CheckCreatedAccountStatusAsync(accountId);

        if (res.HasValue()) {
            await SelectAccountAsync(accountId,
                                     res.Reference,
                                     res.Token,
                                     res.Individual?.Name?.FirstName ?? res.Organization?.Contact?.FirstName,
                                     res.Individual?.Name?.LastName ?? res.Organization?.Contact?.LastName,
                                     res.GetName(_formatter));

            AppendToCache(res);

            await VerifyAccountAccessAsync(accountId);

            return CreatedStatus.ForCreated(res);
        } else {
            return CreatedStatus.ForNotCreated<AccountRes>();
        }
    }

    public async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        var cacheKey = GetCacheKey(email);

        var accounts = await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry => {
            cacheEntry.AbsoluteExpirationRelativeToNow = CacheDuration;

            return await FindAccountsWithEmailAsync(email);
        });

        return accounts;
    }

    public async Task UpdateAccountAsync(AccountReq account) {
        await VerifyAccountAccessAsync(account.Id);
        await VerifyEmailMatchesMemberAsync(account.Email.Address);
        
        await UpdateExistingAccountAsync(account);
        
        await SelectAccountAsync(account.Id,
                                 account.Reference,
                                 account.GetToken(_formatter),
                                 account.Individual?.Name?.FirstName ?? account.Organization?.Contact?.FirstName,
                                 account.Individual?.Name?.LastName ?? account.Organization?.Contact?.LastName,
                                 account.GetName(_formatter));
    }

    private void AppendToCache(AccountRes res) {
        var cacheKey = GetCacheKey(res.Email.Address);

        if (MemoryCache.TryGetValue(cacheKey, out IEnumerable<AccountRes> existing)) {
            MemoryCache.Set(cacheKey, existing.Concat(res).ToList(), CacheDuration);
        }
    }
    
    private string GetCacheKey(string email) {
        return email.ToLowerInvariant();
    }

    private async Task SelectAccountAsync(string accountId,
                                          string accountReference,
                                          string accountToken,
                                          string accountFirstName,
                                          string accountLastName,
                                          string accountDisplayName) {
        _accountCookie.Set(accountId, accountReference, accountToken);

        if (accountDisplayName.HasValue()) {
            await SetMemberNameAsync(accountDisplayName, accountFirstName, accountLastName);
        }
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

    private async Task SetMemberNameAsync(string accountName, string accountFirstName, string accountLastName) {
        var memberIdentityUser = await _memberManager.GetCurrentMemberAsync();
        var member = _memberService.GetByKey(memberIdentityUser.Key);

        if (!member?.Name.EqualsInvariant(accountName) == true) {
            member.Name = accountName;
            
            if (member.HasProperty(MemberConstants.Member.Properties.FirstName) && accountFirstName.HasValue()){
                member.SetValue(MemberConstants.Member.Properties.FirstName, accountFirstName);
            }
            
            if (member.HasProperty(MemberConstants.Member.Properties.LastName) && accountLastName.HasValue()){
                member.SetValue(MemberConstants.Member.Properties.LastName, accountLastName);
            }

            _memberService.Save(member);
        }
    }

    private async Task<AccountRes> CheckCreatedAccountStatusAsync(string accountId) {
        var client = await GetClientAsync();

        try {
            var connectAccountRes = await client.InvokeAsync(x => x.GetAccountCreatedStatusAsync(accountId));
            var accountRes = _mapper.Map<ConnectAccountRes, AccountRes>(connectAccountRes);
            
            return accountRes;
        } catch (CloudApiException ex) when (ex.InnerException is ApiException apiException) {
            if (apiException.StatusCode == StatusCodes.Status404NotFound) {
                return null;
            }

            throw;
        }
    }
    
    private async Task<string> CreateNewAccountAsync(AccountReq account) {
        var req = _mapper.Map<IAccount, ConnectAccountReq>(account);
        var client = await GetClientAsync();

        var res = await client.InvokeAsync(x => x.CreateAccountAsync(req));

        return res;
    }

    private async Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email) {
        var client = await GetClientAsync();

        var criteria = new ConnectEmailCriteria();
        criteria.Address = email;

        var connectAccountResCollection = await client.InvokeAsync(x => x.FindMatchesByEmailAsync(criteria));
        var accountResCollection = connectAccountResCollection.Select(_mapper.Map<ConnectAccountRes, AccountRes>)
                                                              .ToList();

        return accountResCollection;
    }

    private async Task UpdateExistingAccountAsync(AccountReq account) {
        var req = _mapper.Map<IAccount, ConnectAccountReq>(account);
        var client = await GetClientAsync();

        await client.InvokeAsync(x => x.UpdateAccountAsync(account.Id, req));
    }

    private async Task<CloudApiClient<AccountsClient>> GetClientAsync() {
        if (_client == null) {
            _client = await _clientFactory.CreateAsync(UmbracoAuthTypes.Member, CloudApiTypes.Engage);
        }

        return _client;
    }
}