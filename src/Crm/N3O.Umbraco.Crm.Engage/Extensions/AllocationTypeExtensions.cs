using N3O.Umbraco.Giving.Lookups;
using System;
using EngageAllocationType = N3O.Umbraco.Crm.Engage.Clients.AllocationType;

namespace N3O.Umbraco.Crm.Engage.Extensions;

public static class AllocationTypeExtensions {
    public static EngageAllocationType ToEngageAllocationType(this AllocationType allocationType) {
        return (EngageAllocationType) Enum.Parse(typeof(EngageAllocationType), allocationType.Id, true);
    }
}