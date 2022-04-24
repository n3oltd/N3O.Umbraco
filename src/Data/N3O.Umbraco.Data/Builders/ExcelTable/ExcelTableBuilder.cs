using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Builders {
    public class ExcelTableBuilder : IExcelTableBuilder {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILookups _lookups;
        private readonly IEnumerable<IDefaultExcelCellConverter> _defaultExcelCellConverters;

        public ExcelTableBuilder(IServiceProvider serviceProvider,
                                 ILookups lookups,
                                 IEnumerable<IDefaultExcelCellConverter> defaultExcelCellConverters) {
            _serviceProvider = serviceProvider;
            _lookups = lookups;
            _defaultExcelCellConverters = defaultExcelCellConverters;
        }

        public IFluentExcelTableBuilder ForTable(ITable table) {
            return new FluentExcelTableBuilder(_serviceProvider,
                                               _lookups,
                                               _defaultExcelCellConverters,
                                               table);
        }
    }
}