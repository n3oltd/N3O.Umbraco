using N3O.Umbraco.Content;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Extensions; 

public partial class ContentHelperExtensions {
    public static T GetDataListValue<T>(this IContentHelper contentHelper,
                                        IContent content,
                                        string propertyAlias) {
        return contentHelper.GetConvertedValue<DataListValueConverter, T>(content.ContentType.Alias,
                                                                          propertyAlias,
                                                                          content.GetValue(propertyAlias));
    }
    
    public static IEnumerable<T> GetDataListValues<T>(this IContentHelper contentHelper,
                                                      IContent content,
                                                      string propertyAlias) {
        return contentHelper.GetConvertedValue<DataListValueConverter, IEnumerable<T>>(content.ContentType.Alias,
                                                                                       propertyAlias,
                                                                                       content.GetValue(propertyAlias));
    }
}