using Humanizer;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Blocks {
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
            var path = $"/Views/Blocks/{blockAlias.Pascalize()}/{_name.Pascalize()}";

            var definition = new LayoutDefinition(id,
                                                  _name,
                                                  _description,
                                                  $"{path}.png",
                                                  $"{path}.cshtml");

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
}
