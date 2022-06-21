using System;
using Umbraco.Cms.Core;

namespace N3O.Umbraco.Extensions;

public static class UdiExtensions {
    public static Guid? ToId(this Udi udi) {
        return (udi as GuidUdi)?.Guid;
    }
}
