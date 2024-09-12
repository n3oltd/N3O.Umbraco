using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Context;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Subscription;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm.Engage;

// TODO SHagufta, implement this
public class EngageAccountManager : AccountManager {
    private readonly AccountCookie _accountCookie;
    private readonly ClientFactory<AccountsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public EngageAccountManager(AccountCookie accountCookie,
                                ClientFactory<AccountsClient> clientFactory,
                                ISubscriptionAccessor subscriptionAccessor) : base(accountCookie) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override async Task CreateAccountAsync(AccountReq account) {
        // Forward the request to Engage, only bit that needs some thought is how we bubble up errors via
        // the validation/problem details mechanism
        throw new NotImplementedException();
    }

    public override async Task<IEnumerable<AccountRes>> FindAccountsByEmailAsync(string email) {
        var subscription = _subscriptionAccessor.GetSubscription();

        Client = await _clientFactory.CreateAsync(subscription);

        var res = await Client.InvokeAsync<ICollection<AccountRes>>(x => x.FindMatchesByEmailAsync, email, CancellationToken.None);

        return res;
    }

    public override async Task UpdateAccountAsync(AccountReq account) {
        throw new NotImplementedException();
    }

    protected ServiceClient<AccountsClient> Client { get; private set; }
}