using N3O.Umbraco.Authentication.Auth0;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Crm.Engage.NamedParameters;
using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crm.Engage;

public class UserDirectoryIdAccessor : IUserDirectoryIdAccessor {
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private readonly IMemberManager _memberManager;
    private readonly IUserDirectory _userDirectory;
    private readonly UserDirectoryId _userDirectoryId;
    private string _value;

    public UserDirectoryIdAccessor(IMemberManager memberManager,
                                   IUserDirectory userDirectory,
                                   UserDirectoryId userDirectoryId = null) {
        _memberManager = memberManager;
        _userDirectory = userDirectory;
        _userDirectoryId = userDirectoryId;
    }

    public async Task<string> GetIdAsync() {
        if (_value == null) {
            _value ??= _userDirectoryId?.Value;

            if (_value == null) {
                var currentMember = await _memberManager.GetCurrentPublishedMemberAsync();

                if (currentMember != null) {
                    _value = await Cache.GetOrAddAtomicAsync(currentMember.Email, async () => {
                        var directoryUser = await _userDirectory.GetUserByEmailAsync(ClientTypes.Members, currentMember.Email);

                        return directoryUser?.UserId;
                    });
                }
            }
        }

        return _value;
    }
}