using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Builders {
    public class ColumnRangeBuilder : IColumnRangeBuilder {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFormatter _formatter;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly ILocalClock _localClock;
        private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;

        public ColumnRangeBuilder(IServiceProvider serviceProvider,
                                  IFormatter formatter,
                                  IStringLocalizer stringLocalizer,
                                  ILocalClock localClock,
                                  ILocalizationSettingsAccessor localizationSettingsAccessor) {
            _serviceProvider = serviceProvider;
            _formatter = formatter;
            _stringLocalizer = stringLocalizer;
            _localClock = localClock;
            _localizationSettingsAccessor = localizationSettingsAccessor;
        }
        
        public IFluentColumnRangeBuilder<TValue> Bool<TValue>() {
            return OfType<TValue>(OurDataTypes.Bool);
        }

        public IFluentColumnRangeBuilder<TValue> Date<TValue>() {
            return OfType<TValue>(OurDataTypes.Date);
        }

        public IFluentColumnRangeBuilder<TValue> DateTime<TValue>() {
            return OfType<TValue>(OurDataTypes.DateTime);
        }

        public IFluentColumnRangeBuilder<TValue> Decimal<TValue>() {
            return OfType<TValue>(OurDataTypes.Decimal);
        }

        public IFluentColumnRangeBuilder<TValue> Integer<TValue>() {
            return OfType<TValue>(OurDataTypes.Integer);
        }

        public IFluentColumnRangeBuilder<TValue> Lookup<TValue>() {
            return OfType<TValue>(OurDataTypes.Lookup);
        }

        public IFluentColumnRangeBuilder<TValue> Reference<TValue>() {
            return OfType<TValue>(OurDataTypes.Reference);
        }

        public IFluentColumnRangeBuilder<TValue> Money<TValue>() {
            return OfType<TValue>(OurDataTypes.Money);
        }

        public IFluentColumnRangeBuilder<TValue> String<TValue>() {
            return OfType<TValue>(OurDataTypes.String);
        }

        public IFluentColumnRangeBuilder<TValue> Time<TValue>() {
            return OfType<TValue>(OurDataTypes.Time);
        }

        public IFluentColumnRangeBuilder<TValue> OfType<TValue>(DataType dataType) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            var builder = new FluentColumnRangeBuilder<TValue>(_serviceProvider,
                                                               _formatter,
                                                               _stringLocalizer,
                                                               localizationSettings,
                                                               _localClock,
                                                               dataType);

            return builder;
        }
    }
}