using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions;

public static class MemberManagerExtensions {
    public static async Task<string> GetCurrentPublishedMemberEmailAsync(this IMemberManager memberManager) {
        var member = await GetCurrentPublishedMemberAsync(memberManager);

        return member?.Value("email")?.ToString();
    }

    public static async Task<IPublishedContent> GetCurrentPublishedMemberAsync(this IMemberManager memberManager) {
        var memberIdentity = await memberManager.GetCurrentMemberAsync();
        
        if (memberIdentity == null) {
            return null;
        }

        var member = memberManager.AsPublishedMember(memberIdentity);

        return member;
    }
}