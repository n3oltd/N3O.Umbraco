using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.References;
using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCrowdfunderInfo : Value, ICrowdfunderInfo {
    public WebhookCrowdfunderInfo(Guid id,
                                  WebhookReference reference,
                                  WebhookLookup type,
                                  WebhookFundraiserInfo fundraiser) {
        Id = id;
        Reference = reference;
        Type = type;
        Fundraiser = fundraiser;
    }

    public Guid Id { get; }
    public WebhookReference Reference { get; }
    public WebhookLookup Type { get; }
    public WebhookFundraiserInfo Fundraiser { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Reference;
        yield return Type;
        yield return Fundraiser;
    }

    [JsonIgnore]
    Reference ICrowdfunderInfo.Reference => Reference.ToReference();

    [JsonIgnore]
    CrowdfunderType ICrowdfunderInfo.Type => StaticLookups.FindById<CrowdfunderType>(Type.Id);
    
    [JsonIgnore]
    IFundraiserInfo ICrowdfunderInfo.Fundraiser => Fundraiser;
}