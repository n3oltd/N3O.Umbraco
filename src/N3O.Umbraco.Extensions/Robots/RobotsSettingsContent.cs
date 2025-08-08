using N3O.Umbraco.Content;

namespace N3O.Umbraco.Robots;

public class RobotsSettingsContent : UmbracoContent<RobotsSettingsContent> {
    public string CustomDirectives => GetValue(x => x.CustomDirectives);
}