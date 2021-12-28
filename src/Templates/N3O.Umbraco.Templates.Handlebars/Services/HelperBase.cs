using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System;

namespace N3O.Umbraco.Templates.Handlebars;

public abstract class HelperBase {
    private readonly IJsonProvider _jsonProvider;
    private readonly int _minArgs;
    private readonly int _maxArgs;

    protected HelperBase(ILogger logger, IJsonProvider jsonProvider, int args)
        : this(logger, jsonProvider, args, args) { }

    protected HelperBase(ILogger logger, IJsonProvider jsonProvider, int minArgs, int maxArgs) {
        _jsonProvider = jsonProvider;
        _minArgs = minArgs;
        _maxArgs = maxArgs;
        
        Logger = logger;
    }

    protected void Try(Arguments args, Action<HandlebarsArguments> action) {
        try {
            if (args.Length < _minArgs || args.Length > _maxArgs) {
                throw new ArgumentOutOfRangeException(nameof(args));
            }
            
            var handlebarsArgs = new HandlebarsArguments(_jsonProvider, args);

            action(handlebarsArgs);
        } catch (Exception ex) {
            var serializedArgs = _jsonProvider.SerializeObject(args);

            Logger.LogError(ex, $"Error whilst processing {GetType().GetFriendlyName()} with arguments {serializedArgs}");
        }
    }
    
    protected ILogger Logger { get; }
}
