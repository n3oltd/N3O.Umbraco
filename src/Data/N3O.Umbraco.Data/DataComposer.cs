using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.ContentApps;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Extensions;
using OfficeOpenXml;
using System.Text;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Data {
    public class DataComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            RegisterApis(builder);
            RegisterConverters(builder);
            RegisterExports(builder);
            RegisterImports(builder);
            RegisterTables(builder);
        }

        private void RegisterApis(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Content);
            builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Export);
            builder.Services.AddOpenApiDocument(DataConstants.ApiNames.Import);
        }
        
        private void RegisterConverters(IUmbracoBuilder builder) {
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
            builder.Services.AddTransient<IImportProcessingQueue, ImportProcessingQueue>();
            
            builder.Components().Append<ImportsMigrationsComponent>();

            RegisterAll(t => t.ImplementsInterface<IImportContentFilter>(),
                        t => builder.Services.AddTransient(typeof(IImportContentFilter), t));
            
            RegisterAll(t => t.ImplementsInterface<IImportPropertyFilter>(),
                        t => builder.Services.AddTransient(typeof(IImportPropertyFilter), t));
            
            builder.ContentApps().Append<ImportApp>();
        }
        
        private void RegisterTables(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IColumnRangeBuilder, ColumnRangeBuilder>();
            builder.Services.AddTransient<IColumnVisibility, ColumnVisibility>();
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
}