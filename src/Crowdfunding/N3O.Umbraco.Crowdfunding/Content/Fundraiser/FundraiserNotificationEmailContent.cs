using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserNotificationEmail.Alias)]
public class FundraiserNotificationEmailContent : UmbracoContent<FundraiserNotificationEmailContent> {
    public string Body => GetValue(x => x.Body);
    public DateTime SentAt => GetValue(x => x.SentAt);
    public string Subject => GetValue(x => x.Subject);
    public string To => GetValue(x => x.To);
    public string Type => GetValue(x => x.Type);
}