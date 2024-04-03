using N3O.Umbraco.Content;
using N3O.Umbraco.ImageProcessing.Models;

namespace N3O.Umbraco.ImageProcessing.Content;

public class RoundedCornersOperationElement : UmbracoElement<RoundedCornersOperationElement>, IHoldSize {
    public int CornerRadius => GetValue(x => x.CornerRadius);
    public int? Height => GetValue(x => x.Height);
    public int? Width => GetValue(x => x.Width);
}
