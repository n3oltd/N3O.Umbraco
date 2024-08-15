using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Extensions;

public static class MemberExtensions {
    public static async Task<IPublishedContent> GetCurrentMemberAsync(IMemberManager memberManager) {
        var memberIdentity = await memberManager.GetCurrentMemberAsync();
        
        if (memberIdentity == null) {
            return null;
        }

        var member = memberManager.AsPublishedMember(memberIdentity);

        return member;
    }
}