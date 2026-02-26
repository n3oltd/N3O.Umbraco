using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.Linq;
using Umbraco.Engage.Infrastructure.Personalization.PersonalizationProfile;
using Umbraco.Engage.Infrastructure.Personalization.Segments.Rules;

namespace N3O.Umbraco.Cloud.Platforms.Marketing;

public class TelethonOnAirSegmentRule : BaseSegmentRule {
    private readonly ILocalClock _localClock;
    private readonly ICdnClient _cdnClient;

    public TelethonOnAirSegmentRule(ILocalClock localClock,
                                    ICdnClient cdnClient,
                                    long id,
                                    Guid key,
                                    long segmentId,
                                    string type,
                                    string config,
                                    bool isNegation,
                                    DateTime created,
                                    DateTime? updated)
        : base(id, key, segmentId, type, config, isNegation, created, updated) {
        _localClock = localClock;
        _cdnClient = cdnClient;
    }
    
    public override SegmentRuleValidationMode ValidationMode => SegmentRuleValidationMode.Always;

    public override bool IsSatisfied(IPersonalizationProfile context) {
        var now = _localClock.GetLocalNow();

        var publishedCampaigns = _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                 JsonSerializers.JsonProvider)
                                           .GetAwaiter()
                                           .GetResult();

        return publishedCampaigns.OrEmpty(x => x.Campaigns)
                                 .Any(x => x.Type == CampaignType.Telethon && IsOnAir(now, x.Telethon));
    }

    private bool IsOnAir(LocalDateTime now, PublishedTelethonCampaign telethon) {
        var start = JsonConvert.DeserializeObject<DateTime>(telethon.Begin).ToLocalDateTime();
        var end = JsonConvert.DeserializeObject<DateTime>(telethon.End).ToLocalDateTime();

        return now >= start && now <= end;
    }
}
