namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingEnvironmentRes {
    public string Name { get; set; }
    public string ApiKey { get; set; }
    public string Domain { get; set; }
    public bool Default { get; set; }
    
    public static CrowdfundingEnvironmentRes For(string name,
                                                 string apiKey,
                                                 string domain,
                                                 bool @default) {
        var res = new CrowdfundingEnvironmentRes();
        
        res.Name = name;
        res.ApiKey = apiKey;
        res.Domain = domain;
        res.Default = @default;

        return res;
    }
}