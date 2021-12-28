using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace N3O.Umbraco.Analytics {
    public interface IDataLayerBuilder {
        HtmlString BuildJavaScript(IEnumerable<object> toPush);
    }
}
