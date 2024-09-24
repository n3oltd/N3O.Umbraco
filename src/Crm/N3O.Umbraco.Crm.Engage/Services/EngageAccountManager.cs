using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm.Engage;

public class EngageAccountManager : AccountManager {
    private readonly ClientFactory<AccountsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private ServiceClient<AccountsClient> _client;

    public EngageAccountManager(AccountCookie accountCookie,
                                IMemberManager memberManager,
                                ClientFactory<AccountsClient> clientFactory,
                                ISubscriptionAccessor subscriptionAccessor,
                                IFormatter formatter)
        : base(memberManager, accountCookie, formatter) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
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
            
            _client = await _clientFactory.CreateAsync(subscription);
        }

        return _client;
    }
}