using N3O.Umbraco.Localization;
using System;
using Umbraco.Engage.Infrastructure.Personalization.Segments;
using Umbraco.Engage.Infrastructure.Personalization.Segments.Rules;

namespace N3O.Umbraco.Cloud.Platforms.Marketing;

public class TelethonOnAirSegmentRuleFactory : ISegmentRuleFactory {
    private readonly ILocalClock _localClock;
    private readonly ICdnClient _cdnClient;

    public TelethonOnAirSegmentRuleFactory(ILocalClock localClock, ICdnClient cdnClient) {
        _localClock = localClock;
        _cdnClient = cdnClient;
    }

    public ISegmentRule CreateRule(string config,
                                   bool isNegation,
                                   long id,
                                   long segmentId,
                                   DateTime created,
                                   DateTime? updated) {
        return CreateRule(config, isNegation, id, Guid.NewGuid(), segmentId, created, updated);
    }

    public ISegmentRule CreateRule(string config,
                                   bool isNegation,
                                   long id,
                                   Guid key,
                                   long segmentId,
                                   DateTime created,
                                   DateTime? updated) {
        return new TelethonOnAirSegmentRule(_localClock,
                                            _cdnClient,
                                            id,
                                            key,
                                            segmentId,
                                            RuleType,
                                            config,
                                            isNegation,
                                            created,
                                            updated);
    }

    public string RuleType => "TelethonOnAir";

}
