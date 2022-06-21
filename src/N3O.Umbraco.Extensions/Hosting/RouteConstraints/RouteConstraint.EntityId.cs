using System;

namespace N3O.Umbraco.Hosting;

internal class EntityIdRouteConstraint : RouteConstraint {
    protected override bool IsValid(string value) {
        return Guid.TryParse(value, out _);
    }
}
