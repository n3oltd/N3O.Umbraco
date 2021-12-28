using N3O.Umbraco.Extensions;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Blocks;

public class LayoutBuilder : ILayoutBuilder {
    private string _name;
    private string _description;

    public void SetDescription(string description) {
        _description = description;
    }
    
    public void SetName(string name) {
        _name = name;
    }
    
    public LayoutDefinition Build(string blockAlias) {
        Validate();
        
        var id = (blockAlias + _name).GetDeterministicHashCode(true).ToGuid();

        var definition = new LayoutDefinition(id,
                                              _name,
                                              _description,
                                              $"/Views/Blocks/{blockAlias}/preview.png",
                                              $"/Views/Blocks/{blockAlias}/preview.png");

        return definition;
    }

    private void Validate() {
        EnsureHasValue(_name, "name");
        EnsureHasValue(_description, "description");
    }
    
    private void EnsureHasValue<T>(T obj, string name) {
        if (obj == null) {
            throw new Exception($"{name} must be specified");
        }
    }
}
