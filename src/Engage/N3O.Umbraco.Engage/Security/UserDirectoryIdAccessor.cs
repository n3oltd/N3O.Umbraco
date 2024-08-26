using N3O.Umbraco.Authentication.Auth0;
using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using Umbraco.Cms.Core.Security;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Engage.NamedParameters;

namespace N3O.Umbraco.Engage.Security;

public class UserDirectoryIdAccessor : IUserDirectoryIdAccessor {
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IUserDirectory _userDirectory;
    private readonly UserDirectoryId _userDirectoryId;
    private string _value;

    public UserDirectoryIdAccessor(IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                                   IUserDirectory userDirectory,
                                   UserDirectoryId userDirectoryId = null) {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _userDirectory = userDirectory;
        _userDirectoryId = userDirectoryId;
    }

    public async Task<string> GetIdAsync() {
        if (_value == null) {
            _value ??= _userDirectoryId?.Value;

            if (_value == null) {
                var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;

                if (currentUser != null) {
                    _value = await Cache.GetOrAddAtomicAsync(currentUser.Email, async () => {
                        var directoryUser = await _userDirectory.GetUserByEmailAsync(ClientTypes.Members, currentUser.Email);

                        return directoryUser?.UserId;
                    });
                }
            }
        }

        return _value;
    }
}