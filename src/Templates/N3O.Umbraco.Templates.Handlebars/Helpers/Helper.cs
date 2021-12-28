using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.Helpers {
    public abstract class Helper : HelperBase, IHelper {
        protected Helper(ILogger logger, IJsonProvider jsonProvider, int args) : base(logger, jsonProvider, args) { }

        protected Helper(ILogger logger, IJsonProvider jsonProvider, int minArgs, int maxArgs) :
            base(logger, jsonProvider, minArgs, maxArgs) { }

        public abstract string Name { get; }

        public void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, Arguments args) {
            Try(args, x => Execute(writer, context, x));
        }

        protected abstract void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, HandlebarsArguments args);
    }
}
