using N3O.Umbraco.Entities;

namespace N3O.Umbraco.Hosting;

internal class RevisionIdRouteConstraint : RouteConstraint {
    protected override bool IsValid(string value) {
        return RevisionId.IsValid(value);
    }
}
