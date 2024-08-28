using N3O.Umbraco.Authentication.Auth0;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Crm.Engage.NamedParameters;
using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crm.Engage;

public class UserDirectoryIdAccessor : IUserDirectoryIdAccessor {
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IUserDirectory _userDirectory;
    private readonly UserDirectoryId _userDirectoryId;
    private string _value;

    public UserDirectoryIdAccessor(IMemberManager memberManager,
                                   IUserDirectory userDirectory,
                                   IMemberService memberService,
                                   UserDirectoryId userDirectoryId = null) {
        _memberManager = memberManager;
        _userDirectory = userDirectory;
        _memberService = memberService;
        _userDirectoryId = userDirectoryId;
    }

    public async Task<string> GetIdAsync() {
        if (_value == null) {
            _value ??= _userDirectoryId?.Value;

            if (_value == null) {
                var publishedMember = await _memberManager.GetCurrentPublishedMemberAsync();
                var member = _memberService.GetById(publishedMember.Id);
                
                if (member != null) {
                    _value = await Cache.GetOrAddAtomicAsync(member.Email, async () => {
                        var directoryUser = await _userDirectory.GetUserByEmailAsync(ClientTypes.Members, member.Email);

                        return directoryUser?.UserId;
                    });
                }
            }
        }

        return _value;
    }
}