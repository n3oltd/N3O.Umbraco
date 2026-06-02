using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco;

public interface IContentmentDataSource : global::Umbraco.Community.Contentment.DataEditors.IContentmentDataSource,
                                          IDataSourceValueConverter { }