using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm.Engage;

public class EngageAccountManager : AccountManager {
    private readonly IMemberManager _memberManager;
    private readonly ClientFactory<AccountsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private ServiceClient<AccountsClient> _client;

    public EngageAccountManager(AccountCookie accountCookie,
                                IMemberManager memberManager,
                                ClientFactory<AccountsClient> clientFactory,
                                ISubscriptionAccessor subscriptionAccessor)
        : base(memberManager, accountCookie) {
        _memberManager = memberManager;
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override async Task<string> CreateAccountAsync(AccountReq account) {
        // Forward the request to Engage, only bit that needs some thought is how we bubble up errors via
        // the validation/problem details mechanism
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<AccountReq, string>(x => x.CreateAccountAsync, account, CancellationToken.None);

        return res;
    }

    protected override async Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email) {
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<ICollection<AccountRes>>(x => x.FindMatchesByEmailAsync, email);

        return res;
    }

    public override async Task<AccountRes> UpdateAccountAsync(AccountReq account) {
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<AccountReq, AccountRes>(x => x.UpdateAccountAsync, account.Id, account, CancellationToken.None);

        return res;
    }

    private async Task<ServiceClient<AccountsClient>> GetClientAsync() {
        if (_client == null) {
            var subscription = _subscriptionAccessor.GetSubscription();
            _client = await _clientFactory.CreateAsync(subscription);
        }

        return _client;
    }
}