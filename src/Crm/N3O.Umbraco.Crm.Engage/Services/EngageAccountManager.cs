using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crm.Engage.Exceptions;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crm.Engage;

public class EngageAccountManager : AccountManager {
    private readonly ClientFactory<AccountsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private ServiceClient<AccountsClient> _client;

    public EngageAccountManager(AccountCookie accountCookie,
                                IMemberManager memberManager,
                                IMemberService memberService,
                                ClientFactory<AccountsClient> clientFactory,
                                ISubscriptionAccessor subscriptionAccessor,
                                IFormatter formatter)
        : base(memberManager, memberService, accountCookie, formatter) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
    }
    
    protected override async Task<AccountRes> CheckCreatedAccountStatusAsync(string accountId) {
        var client = await GetClientAsync();

        try {
            var res = await client.InvokeAsync<AccountRes>(x => x.GetAccountCreatedStatusAsync, accountId);

            return res;
        } catch (ServiceClientException ex) when (ex.InnerException is ApiException apiException) {
            if (apiException.StatusCode == StatusCodes.Status404NotFound) {
                return null;
            }

            throw;
        }
    }

    protected override async Task<string> CreateNewAccountAsync(AccountReq account) {
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<AccountReq, string>(x => x.CreateAccountAsync, account);

        return res;
    }

    protected override async Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email) {
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<ICollection<AccountRes>>(x => x.FindMatchesByEmailAsync, email);

        return res;
    }

    protected override async Task UpdateExistingAccountAsync(AccountReq account) {
        var client = await GetClientAsync();

        await client.InvokeAsync(x => x.UpdateAccountAsync, account.Id, account);
    }

    private async Task<ServiceClient<AccountsClient>> GetClientAsync() {
        if (_client == null) {
            var subscription = _subscriptionAccessor.GetSubscription();
            
            _client = await _clientFactory.CreateAsync(subscription, ClientTypes.Members);
        }

        return _client;
    }
}