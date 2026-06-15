using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.DataTypes;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Providers;
using N3O.Umbraco.Data.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using OfficeOpenXml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        ExcelPackage.License.SetNonCommercialOrganization("N3O");

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

    }

    private void RegisterImports(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IImportQueue, ImportQueue>();
        builder.Services.AddTransient<IImportProcessingQueue, ImportProcessingQueue>();

        builder.Components().Append<ImportsMigrationsComponent>();

        RegisterAll(t => t.ImplementsInterface<IImportContentFilter>(),
                    t => builder.Services.AddTransient(typeof(IImportContentFilter), t));

        RegisterAll(t => t.ImplementsInterface<IImportPropertyFilter>(),
                    t => builder.Services.AddTransient(typeof(IImportPropertyFilter), t));

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

public class DataComponent : IAsyncComponent {
    private readonly IRuntimeState _runtimeState;
    private readonly IDataTypeService _dataTypeService;
    private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
    private readonly IDataValueEditorFactory _dataValueEditorFactory;
    private readonly IIOHelper _iioHelper;

    public DataComponent(IRuntimeState runtimeState,
                         IDataTypeService dataTypeService,
                         IConfigurationEditorJsonSerializer configurationEditorJsonSerializer,
                         IDataValueEditorFactory dataValueEditorFactory,
                         IIOHelper iioHelper) {
        _runtimeState = runtimeState;
        _dataTypeService = dataTypeService;
        _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
        _dataValueEditorFactory = dataValueEditorFactory;
        _iioHelper = iioHelper;
    }
    
    public async Task InitializeAsync(bool isRestarting, CancellationToken cancellationToken) {
        if (_runtimeState.Level == RuntimeLevel.Run) {
            await EnsureDataTypeExistsAsync(new ImportNoticesViewerDataEditor(_dataValueEditorFactory,
                                                                             _iioHelper),
                                            ImportNoticesViewerDataEditor.DataEditorName);

            await EnsureDataTypeExistsAsync(new ImportDataEditorDataEditor(_dataValueEditorFactory,
                                                                          _iioHelper),
                                            ImportDataEditorDataEditor.DataEditorName);
        }
    }

    private async Task EnsureDataTypeExistsAsync(DataEditor dataEditor, string name) {
        var key = UmbracoId.Generate(IdScope.DataType, dataEditor.Alias);

        var existing = await _dataTypeService.GetAsync(key);

        if (existing != null) {
            if (!existing.Name.EqualsInvariant(name)) {
                existing.Name = name;
                
                await _dataTypeService.UpdateAsync(existing, global::Umbraco.Cms.Core.Constants.Security.SuperUserKey);
            }

            return;
        }

        var dataType = new DataType(dataEditor, _configurationEditorJsonSerializer);
        dataType.Name = name;
        dataType.Key = key;
        dataType.EditorUiAlias = dataEditor.Alias;

        await _dataTypeService.CreateAsync(dataType, global::Umbraco.Cms.Core.Constants.Security.SuperUserKey);
    }

    public Task TerminateAsync(bool isRestarting, CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
