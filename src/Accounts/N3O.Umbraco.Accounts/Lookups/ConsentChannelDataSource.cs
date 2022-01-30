using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Accounts.Lookups {
    public class ConsentChannelDataSource : IDataListSourceValueConverter {
        private readonly ILookups _lookups;

        public ConsentChannelDataSource(ILookups lookups) {
            _lookups = lookups;
        }
        
        public string Name => "Consent Channels";
        public string Description => "Data source for consent channels";
        public string Icon => "icon-sensor";
        public Dictionary<string, object> DefaultValues => default;
        public IEnumerable<ConfigurationField> Fields => default;
        public string Group => "N3O";
        public OverlaySize OverlaySize => OverlaySize.Small;
        
        public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
            return _lookups.GetAll<ConsentChannel>().Select(ToDataListItem).ToList();
        }

        public Type GetValueType(Dictionary<string, object> config) {
            return typeof(ConsentChannel);
        }

        public object ConvertValue(Type type, string value) {
            return _lookups.FindById<ConsentChannel>(value);
        }
        
        private DataListItem ToDataListItem(ConsentChannel consentChannel) {
            var dataListItem = new DataListItem();
            dataListItem.Name = consentChannel.Name;
            dataListItem.Icon = consentChannel.Icon;
            dataListItem.Value = consentChannel.Id;
            dataListItem.Group = "N3O";

            return dataListItem;
        }
    }
}