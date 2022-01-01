using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Blocks {
    public abstract class BlockBuilder : IBlockBuilder {
        private readonly List<ILayoutBuilder> _layoutBuilders = new();
        private readonly List<BlockCategory> _categories = new();
        private readonly List<string> _limitToContentTypes = new();
        private string _alias;
        private string _description;
        private string _name;
        private string _icon;
        private string _folder;

        protected ILayoutBuilder AddLayout() {
            var layoutBuilder = new LayoutBuilder();

            _layoutBuilders.Add(layoutBuilder);

            return layoutBuilder;
        }
    
        protected void AddLayout(string name, string description) {
            AddLayout(layout => {
                layout.SetName(name);
                layout.SetDescription(description);
            });
        }
    
        protected void AddLayout(Action<ILayoutBuilder> configureLayout) {
            var layoutBuilder = new LayoutBuilder();

            configureLayout(layoutBuilder);
        
            _layoutBuilders.Add(layoutBuilder);
        }

        protected void AddToCategory(BlockCategory category) {
            _categories.Add(category);
        }

        protected void InFolder(string folder) {
            _folder = folder;
        }

        protected void LimitTo(string contentTypeAlias) {
            if (!_limitToContentTypes.Contains(contentTypeAlias, true)) {
                _limitToContentTypes.Add(contentTypeAlias);
            }
        }
    
        protected void LimitTo<T>() {
            LimitTo(AliasHelper.ForContentType<T>());
        }

        protected void SingleLayout() {
            AddLayout("Default", "The default layout");
        }
        protected void WithAlias(string alias) {
            if (alias.EndsWith("Block")) {
                throw new Exception($"Alias must not end with {"Block".Quote()} as this will be appended automatically");
            }
        
            _alias = $"{alias}Block";
        }

        protected void WithDescription(string description) {
            _description = description;
        }
        
        protected void WithIcon(string icon) {
            _icon = icon;
        }
    
        protected void WithName(string name) {
            _name = name;
        }
    
        public BlockDefinition Build() {
            Validate();
        
            var id = UmbracoId.Generate(IdScope.Block, _alias);
            var layouts = _layoutBuilders.Select(x => x.Build(_alias)).ToList();

            var definition = new BlockDefinition(id,
                                                 _alias,
                                                 _name,
                                                 _description,
                                                 _icon,
                                                 _folder,
                                                 $"/Views/Blocks/{_alias}/preview.png",
                                                 _categories,
                                                 layouts,
                                                 _limitToContentTypes,
                                                 Enumerable.Empty<string>());

            return definition;
        }

        private void Validate() {
            EnsureHasValue(_alias, "alias");
            EnsureHasValue(_name, "name");
            EnsureHasValue(_icon, "icon");
            EnsureHasValue(_description, "description");

            EnsureAny(_categories, "category");
            EnsureAny(_layoutBuilders, "Layout");
        }

        private void EnsureHasValue<T>(T obj, string name) {
            if (obj == null) {
                throw new Exception($"{name} must be specified");
            }
        }
    
        private void EnsureAny<T>(IReadOnlyList<T> list, string name) {
            if (list.None()) {
                throw new Exception($"At least one {name} must be specified");
            }
        }
    }
}
