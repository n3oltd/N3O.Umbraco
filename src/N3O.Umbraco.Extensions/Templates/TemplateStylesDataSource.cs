using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Templates {
    public class TemplateStylesDataSource : IDataListSourceValueConverter {
        private readonly ILookups _lookups;

        public TemplateStylesDataSource(ILookups lookups) {
            _lookups = lookups;
        }
        
        public string Name => "Template Styles";
        public string Description => "Data source for template styles";
        public string Icon => "icon-brush";
        public Dictionary<string, object> DefaultValues => default;
        public IEnumerable<ConfigurationField> Fields => default;
        public string Group => "N3O";
        public OverlaySize OverlaySize => OverlaySize.Small;
        
        public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
            return _lookups.GetAll<TemplateStyle>().Select(ToDataListItem).ToList();
        }

        public Type GetValueType(Dictionary<string, object> config) {
            return typeof(TemplateStyle);
        }

        public object ConvertValue(Type type, string value) {
            return _lookups.FindById<TemplateStyle>(value);
        }
        
        private DataListItem ToDataListItem(TemplateStyle templateStyle) {
            var dataListItem = new DataListItem();
            dataListItem.Name = templateStyle.Name;
            dataListItem.Description = templateStyle.Description;
            dataListItem.Icon = templateStyle.Icon;
            dataListItem.Value = templateStyle.Id;
            dataListItem.Group = "N3O";

            return dataListItem;
        }
    }
}