using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public class StatisticsEnvironmentRes {
    public StatisticsEnvironmentRes(string name,
                                    string apiKey,
                                    string domain,
                                    bool @default,
                                    StatisticsEnvironmentType statisticsEnvironment) {
        Name = name;
        ApiKey = apiKey;
        Domain = domain;
        Default = @default;
        StatisticsEnvironment = statisticsEnvironment;
    }

    public StatisticsEnvironmentRes(StatisticsEnvironmentContent statisticsEnvironment)
        : this(statisticsEnvironment.Name,
               statisticsEnvironment.ApiKey,
               statisticsEnvironment.Domain,
               statisticsEnvironment.Default,
               statisticsEnvironment.StatisticsEnvironment) { }

    public string Name { get; set; }
    public string ApiKey { get; set; }
    public string Domain { get; set; }
    public bool Default { get; set; }
    public StatisticsEnvironmentType StatisticsEnvironment { get; set; }
}