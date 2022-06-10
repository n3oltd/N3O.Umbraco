using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters {
    public class EditLinkContentMetadataConverter : ContentMetadataConverter<string> {
        private readonly IUrlBuilder _urlBuilder;

        public EditLinkContentMetadataConverter(IUrlBuilder urlBuilder, IColumnRangeBuilder columnRangeBuilder)
            : base(columnRangeBuilder, ContentMetadatas.EditLink) {
            _urlBuilder = urlBuilder;
        }

        public override object GetValue(IContent content) {
            return $"{_urlBuilder.Root().AppendPathSegment("umbraco")}#/content/content/edit/{content.Id}";
        }
        
        protected override string Title => "Edit Link";
    }
}