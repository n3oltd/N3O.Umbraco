using Auth0.ManagementApi;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectoryConnections : IUserDirectoryConnections {
    private readonly IAuth0ClientFactory _clientFactory;
    private readonly ConcurrentDictionary<UserDirectoryType, Lazy<Task<IReadOnlyCollection<string>>>> _domainAliasesByType;

    public UserDirectoryConnections(IAuth0ClientFactory clientFactory) {
        _clientFactory = clientFactory;
        _domainAliasesByType = new ConcurrentDictionary<UserDirectoryType, Lazy<Task<IReadOnlyCollection<string>>>>();
    }

    public async Task<bool> IsFederatedByEmailAsync(UserDirectoryType userDirectoryType, string email) {
        var domainAliases = await GetDomainAliasesAsync(userDirectoryType);

        return domainAliases.Any(x => email.EndsWith($"@{x}", StringComparison.InvariantCultureIgnoreCase));
    }

    private Task<IReadOnlyCollection<string>> GetDomainAliasesAsync(UserDirectoryType userDirectoryType) {
        var lazy = _domainAliasesByType.GetOrAdd(userDirectoryType,
                                                 x => new Lazy<Task<IReadOnlyCollection<string>>>(() => LoadDomainAliasesAsync(x)));

        return lazy.Value;
    }

    private async Task<IReadOnlyCollection<string>> LoadDomainAliasesAsync(UserDirectoryType userDirectoryType) {
        var managementClient = await _clientFactory.GetManagementApiClientAsync(userDirectoryType);

        var pager = await managementClient.Connections.ListAsync(new ListConnectionsQueryParameters {
            Take = 100
        });

        var domainAliases = new List<string>();

        foreach (var connection in pager.CurrentPage.Items) {
            var connectionAliases = ExtractDomainAliases(connection.Options);

            if (connectionAliases != null) {
                domainAliases.AddRange(connectionAliases);
            }
        }

        return domainAliases;
    }

    private static IEnumerable<string> ExtractDomainAliases(Dictionary<string, object> options) {
        if (options == null || !options.TryGetValue("domain_aliases", out var raw) || raw is not JsonElement element) {
            return null;
        }

        if (element.ValueKind != JsonValueKind.Array) {
            return null;
        }

        return element.Deserialize<IEnumerable<string>>();
    }
}
