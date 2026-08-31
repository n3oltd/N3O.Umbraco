using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.DataTypes;

public interface IDataTypeEditor {
    // Finds a data type the way a designer does, by deterministic key and then by name, so a site holding one
    // under a name of its own is not read as not having it
    IDataType Find(string name);
    T New<T>(string name) where T : IDataTypeDesigner;
}
