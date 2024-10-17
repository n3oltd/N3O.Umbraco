using N3O.Umbraco.Content;

namespace N3O.Umbraco.Crowdfunding.Content;

public class MemberContent : UmbracoContent<MemberContent> {
    public string Name => Content().Name;
    public string AvatarLink => GetValue(x => x.AvatarLink);
    public string Email => GetValue(x => x.Email);
}