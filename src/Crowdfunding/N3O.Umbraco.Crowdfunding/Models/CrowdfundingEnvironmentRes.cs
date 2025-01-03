using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingEnvironmentRes {
    public CrowdfundingEnvironmentRes(string name,
                                    string apiKey,
                                    string domain,
                                    bool @default,
                                    CrowdfundingEnvironmentType crowdfundingEnvironment) {
        Name = name;
        ApiKey = apiKey;
        Domain = domain;
        Default = @default;
        CrowdfundingEnvironment = crowdfundingEnvironment;
    }

    public CrowdfundingEnvironmentRes(EnvironmentContent environment)
        : this(environment.Name,
               environment.ApiKey,
               environment.Domain,
               environment.Default,
               environment.Environment) { }

    public string Name { get; set; }
    public string ApiKey { get; set; }
    public string Domain { get; set; }
    public bool Default { get; set; }
    public CrowdfundingEnvironmentType CrowdfundingEnvironment { get; set; }
}