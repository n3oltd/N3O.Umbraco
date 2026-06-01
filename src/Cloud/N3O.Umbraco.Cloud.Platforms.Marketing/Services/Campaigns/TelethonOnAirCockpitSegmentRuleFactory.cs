// Umbraco.Engage namespaces changed in v17 (BLOCKER-04).
// Umbraco.Engage.Infrastructure.Personalization.Segments → updated namespace in Engage v17
// Umbraco.Engage.Web.Cockpit.Segments → updated namespace in Engage v17
//
// The ICockpitSegmentRuleFactory.TryCreate signature also changed (CockpitSegmentRule is now nullable).
//
// TODO: After upgrading to Umbraco.Engage v17.2.2 and completing the namespace port (BLOCKER-04),
// restore this implementation with updated using directives and the corrected TryCreate signature:
//   public bool TryCreate(ISegmentRule segmentRule, bool isSatisfied, out CockpitSegmentRule? cockpitSegmentRule)
namespace N3O.Umbraco.Cloud.Platforms.Marketing;
