using Perplex.ContentBlocks.Definitions;
using System;

namespace N3O.Umbraco.Blocks {
    public class LayoutDefinition : Value, IContentBlockLayout {
        public LayoutDefinition(Guid id, string name, string description, string previewImage, string viewPath) {
            Id = id;
            Name = name;
            Description = description;
            PreviewImage = previewImage;
            ViewPath = viewPath;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string PreviewImage { get; }
        public string ViewPath { get; }
    }
}