using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System.Buffers;

namespace N3O.Umbraco.Hosting {
    public class OurMvcJsonFormatterOptions : IConfigureOptions<MvcOptions> {
        private readonly ILoggerFactory _loggerFactory;
        private readonly MvcNewtonsoftJsonOptions _jsonOptions;
        private readonly ArrayPool<char> _charPool;
        private readonly ObjectPoolProvider _objectPoolProvider;

        public OurMvcJsonFormatterOptions(ILoggerFactory loggerFactory,
                                          IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
                                          ArrayPool<char> charPool,
                                          ObjectPoolProvider objectPoolProvider) {
            _loggerFactory = loggerFactory;
            _jsonOptions = jsonOptions.Value;
            _charPool = charPool;
            _objectPoolProvider = objectPoolProvider;
        }

        public void Configure(MvcOptions options) {
            ConfigureInputFormatters(options);
            ConfigureOutputFormatters(options);
        }

        private void ConfigureInputFormatters(MvcOptions options) {
            options.InputFormatters.RemoveType<NewtonsoftJsonPatchInputFormatter>();
            options.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();

            var jsonInputLogger = _loggerFactory.CreateLogger<OurJsonInputFormatter>();
            options.InputFormatters.Insert(0, new OurJsonInputFormatter(jsonInputLogger,
                                                                        _jsonOptions.SerializerSettings,
                                                                        _charPool,
                                                                        _objectPoolProvider,
                                                                        options,
                                                                        _jsonOptions));
        }

        private void ConfigureOutputFormatters(MvcOptions options) {
            options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();

            options.OutputFormatters.Insert(0, new OurJsonOutputFormatter(_jsonOptions.SerializerSettings,
                                                                          _charPool,
                                                                          options));
        }
    }
}
