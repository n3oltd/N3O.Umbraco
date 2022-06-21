using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Buffers;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public class OurBodyModelBinder : IModelBinder {
    private readonly BodyModelBinder _bodyModelBinder;

    public OurBodyModelBinder(IHttpRequestStreamReaderFactory readerFactory,
                              ILoggerFactory loggerFactory,
                              IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
                              ArrayPool<char> charPool,
                              ObjectPoolProvider objectPoolProvider,
                              IOptions<MvcOptions> mvcOptions) {
        var jsonInputLogger = loggerFactory.CreateLogger<OurJsonInputFormatter>();
        var formatters = mvcOptions.Value.InputFormatters.ToList();
        
        formatters.Insert(0, new OurJsonInputFormatter(jsonInputLogger,
                                                       jsonOptions.Value.SerializerSettings,
                                                       charPool,
                                                       objectPoolProvider,
                                                       mvcOptions.Value,
                                                       jsonOptions.Value));

        _bodyModelBinder = new BodyModelBinder(formatters, readerFactory, loggerFactory, mvcOptions.Value);
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext) {
        await _bodyModelBinder.BindModelAsync(bindingContext);
    }
}

public class OurBodyModelBinderProvider : IModelBinderProvider {
    public IModelBinder GetBinder(ModelBinderProviderContext context) {
        if (context == null) {
            throw new ArgumentNullException(nameof(context));
        }
        
        if (context.Metadata.BindingSource.IsAnyOf(BindingSource.Body, null)  &&
            OurAssemblies.IsOurAssembly(context.Metadata.ModelType.Assembly)) {
            return new BinderTypeModelBinder(typeof(OurBodyModelBinder));
        }

        return null;
    }
}
