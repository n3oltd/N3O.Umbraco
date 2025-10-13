using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.References;

public class EnvironmentReferenceStartProvider : IReferenceStartProvider{
    public bool CanProvideReference(ReferenceType type) {
        var referenceStart = EnvironmentData.GetOurValue(ReferenceConstants.Environment.Keys.ReferencesStartFromWC);

        if (!referenceStart.HasValue()) {
            return false;
        }
        
        return long.TryParse(referenceStart, out _);
    }
    
    public long GetStartNumber(ReferenceType type) {
        var referenceStart = EnvironmentData.GetOurValue(ReferenceConstants.Environment.Keys.ReferencesStartFromWC);

        long.TryParse(referenceStart, out var startNumber);

        return startNumber;
    }
}