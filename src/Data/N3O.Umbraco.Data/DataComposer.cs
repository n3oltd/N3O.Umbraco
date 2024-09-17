using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.ContentApps;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.DataTypes;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Providers;
using N3O.Umbraco.Data.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using OfficeOpenXml;
using System.Text;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data;

public class DataComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        builder.PropertyValueConverters().Append<ImportNoticesViewerValueConverter>();
        builder.PropertyValueConverters().Append<ImportDataEditorValueConverter>();
        
        builder.Services.AddTransient<ISummaryFieldsBuilder, SummaryFieldsBuilder>();

        RegisterApis(builder);
        RegisterContentSummarisers(builder);
        RegisterConverters(builder);
        RegisterExports(builder);
        RegisterImports(builder);
        RegisterMatchers(builder);
        RegisterTables(builder);
        
        RegisterAll(t => t.ImplementsInterface<IContentPropertyValidator>(),
                    t => builder.Services.AddTransient(typeof(IContentPropertyValidator), t));
        
        builder.Components().Append<DataComponent>();
    }

    private void RegisterApis(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Content);
        builder.Services.AddOpenApiDocument(DataConstants.ApiNames.ContentTypes);
        builder.Services.AddOpenApiDocument(DataConstants.ApiNames.DataTypes);
        builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Exports);
        builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Imports);
    }
    
    private void RegisterContentSummarisers(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IContentSummariser>(),
                    t => builder.Services.AddTransient(typeof(IContentSummariser), t));
    }

    private void RegisterConverters(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IContentMetadataConverter>(),
                    t => builder.Services.AddTransient(typeof(IContentMetadataConverter), t));
        
        RegisterAll(t => t.ImplementsInterface<IPropertyConverter>(),
                    t => builder.Services.AddTransient(typeof(IPropertyConverter), t));
    }

    private void RegisterExports(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IExportContentFilter>(),
                    t => builder.Services.AddTransient(typeof(IExportContentFilter), t));

        RegisterAll(t => t.ImplementsInterface<IExportPropertyFilter>(),
                    t => builder.Services.AddTransient(typeof(IExportPropertyFilter), t));

        builder.ContentApps().Append<ExportApp>();
    }

    private void RegisterImports(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IImportQueue, ImportQueue>();
        builder.Services.AddTransient<IImportProcessingQueue, ImportProcessingQueue>();

        builder.Components().Append<ImportsMigrationsComponent>();

        RegisterAll(t => t.ImplementsInterface<IImportContentFilter>(),
                    t => builder.Services.AddTransient(typeof(IImportContentFilter), t));

        RegisterAll(t => t.ImplementsInterface<IImportPropertyFilter>(),
                    t => builder.Services.AddTransient(typeof(IImportPropertyFilter), t));

        builder.ContentApps().Append<ImportApp>();
    }
    
    private void RegisterMatchers(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IContentMatcher>(),
                    t => builder.Services.AddTransient(typeof(IContentMatcher), t));
    }

    private void RegisterTables(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IColumnRangeBuilder, ColumnRangeBuilder>();
        builder.Services.AddTransient<IExcelCellFormatter, ExcelCellFormatter>();
        builder.Services.AddTransient<IExcelTableBuilder, ExcelTableBuilder>();
        builder.Services.AddTransient<ITableBuilder, TableBuilder>();
        builder.Services.AddTransient<IWorkspace, Workspace>();

        RegisterAll(t => t.ImplementsGenericInterface(typeof(IExcelCellConverter<>)),
                    t => t.GetInterfaces().Concat(t).Do(i => builder.Services.AddTransient(i, t)));

        RegisterAll(t => t.ImplementsGenericInterface(typeof(ICellConverter<>)),
                    t => t.GetInterfaces().Concat(t).Do(i => builder.Services.AddTransient(i, t)));

        RegisterAll(t => t.ImplementsGenericInterface(typeof(ITextConverter<>)),
                    t => t.GetInterfaces().Concat(t).Do(i => builder.Services.AddTransient(i, t)));
    }
}

public class DataComponent : IComponent {
    private readonly IRuntimeState _runtimeState;
    private readonly IDataTypeService _dataTypeService;
    private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
    private readonly IDataValueEditorFactory _dataValueEditorFactory;
    private readonly IEditorConfigurationParser _editorConfigurationParser;
    private readonly IIOHelper _iioHelper;

    public DataComponent(IRuntimeState runtimeState,
                         IDataTypeService dataTypeService,
                         IConfigurationEditorJsonSerializer configurationEditorJsonSerializer,
                         IDataValueEditorFactory dataValueEditorFactory,
                         IEditorConfigurationParser editorConfigurationParser,
                         IIOHelper iioHelper) {
        _runtimeState = runtimeState;
        _dataTypeService = dataTypeService;
        _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
        _dataValueEditorFactory = dataValueEditorFactory;
        _editorConfigurationParser = editorConfigurationParser;
        _iioHelper = iioHelper;
    }
    
    public void Initialize() {
        if (_runtimeState.Level == RuntimeLevel.Run) {
            EnsureDataTypeExists(new ImportNoticesViewerDataEditor(_dataValueEditorFactory,
                                                                   _iioHelper,
                                                                   _editorConfigurationParser));
            
            EnsureDataTypeExists(new ImportDataEditorDataEditor(_dataValueEditorFactory,
                                                                _iioHelper,
                                                                _editorConfigurationParser));
        }
    }

    private void EnsureDataTypeExists(DataEditor dataEditor) {
        if (_dataTypeService.GetDataType(dataEditor.Name) != null) {
            return;
        }
        
        var dataType = new DataType(dataEditor, _configurationEditorJsonSerializer);
        dataType.Name = dataEditor.Name;
        dataType.Key = UmbracoId.Generate(IdScope.DataType, dataEditor.Alias);

        _dataTypeService.Save(dataType);
    }

    public void Terminate() { }
}
