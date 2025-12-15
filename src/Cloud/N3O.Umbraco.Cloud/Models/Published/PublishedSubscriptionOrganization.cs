using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedSubscriptionOrganization {
    public string Name { get; set; }
    public PublishedSubscriptionOrganizationPlatformsSettings PlatformsSettings { get; set; }
}