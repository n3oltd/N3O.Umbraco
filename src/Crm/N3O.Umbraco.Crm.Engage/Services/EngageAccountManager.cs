using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Engage.Clients;
using System;
using System.Collections.Generic;
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

    public override async Task CreateAccountAsync(AccountReq account) {
        // Forward the request to Engage, only bit that needs some thought is how we bubble up errors via
        // the validation/problem details mechanism
        throw new NotImplementedException();
    }

    protected override async Task<IEnumerable<AccountRes>> FindAccountsWithEmailAsync(string email) {
        var client = await GetClientAsync();

        var res = await client.InvokeAsync<ICollection<AccountRes>>(x => x.FindMatchesByEmailAsync, email);

        return res;
    }

    public override async Task UpdateAccountAsync(AccountReq account) {
        throw new NotImplementedException();
    }
    
    private async Task<ServiceClient<AccountsClient>> GetClientAsync() {
        if (_client == null) {
            var subscription = _subscriptionAccessor.GetSubscription();
            _client = await _clientFactory.CreateAsync(subscription);
        }

        return _client;
    }
}