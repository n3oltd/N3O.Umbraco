using Humanizer;
using N3O.Umbraco.Utilities;
using System;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Blocks.Perplex;

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

        var id = UmbracoId.Generate(IdScope.BlockLayout, blockAlias, _name);

        var definition = new LayoutDefinition(id,
                                              _name,
                                              _description,
                                              $"/assets/blocks/{blockAlias.Camelize()}/{ToViewFileName(_name).Camelize()}.png",
                                              $"/Views/Blocks/{blockAlias.Pascalize()}/{ToViewFileName(_name)}.cshtml");

        return definition;
    }

    private static string ToViewFileName(string name) {
        return Regex.Replace(name, @"(?:^|[_\s-]+)(.)", m => m.Groups[1].Value.ToUpper());
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
