// This file tests which types exist in uSync.Publisher v17.3.6
// It will fail to compile with informative errors if types don't exist

using uSync.Publisher.Services;

// Try to reference IPublisherStateService from different namespaces
// If this compiles, the type exists in uSync.Publisher.Services
// If not, we'll see a CS0246 error telling us the type doesn't exist

namespace InspectUsync;

public class TypeCheck {
    // Test if IPublisherStateService exists in uSync.Publisher.Services
    private IPublisherStateService? _test1;
}
