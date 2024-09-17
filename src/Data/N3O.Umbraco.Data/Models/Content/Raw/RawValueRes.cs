using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Data.Models;

public class RawValueRes {
    public HtmlEncodedString Value { get; set; }
    public RawConfigurationRes Configuration { get; set; }
}