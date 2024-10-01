using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.NamedParameters;
using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Authentication.Auth0;

public class UserDirectoryIdAccessor : IUserDirectoryIdAccessor {
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IUserDirectory _userDirectory;
    private readonly UserDirectoryId _userDirectoryId;
    private string _value;

    public UserDirectoryIdAccessor(IMemberManager memberManager,
                                   IUserDirectory userDirectory,
                                   IMemberService memberService,
                                   IUmbracoContextFactory umbracoContextFactory,
                                   UserDirectoryId userDirectoryId = null) {
        _memberManager = memberManager;
        _userDirectory = userDirectory;
        _memberService = memberService;
        _umbracoContextFactory = umbracoContextFactory;
        _userDirectoryId = userDirectoryId;
    }

    public async Task<string> GetIdAsync() {
        if (_value == null) {
            _value ??= _userDirectoryId?.Value;

            if (_value == null) {
                using (_umbracoContextFactory.EnsureUmbracoContext()) {
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
        }

        return _value;
    }
}