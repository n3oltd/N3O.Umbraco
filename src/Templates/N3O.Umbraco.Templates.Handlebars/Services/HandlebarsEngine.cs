using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace N3O.Umbraco.Templates.Handlebars {
    public class HandlebarsEngine : ITemplateEngine {
        private readonly ILogger _logger;
        private readonly IHandlebarsCompiler _handlebarsCompiler;

        public HandlebarsEngine(ILogger logger, IHandlebarsCompiler handlebarsCompiler) {
            _logger = logger;
            _handlebarsCompiler = handlebarsCompiler;
        }

        public bool IsSyntaxValid(string content) {
            var isValid = _handlebarsCompiler.IsSyntaxValid(content);

            return isValid;
        }

        public string Render(string markup, object model) {
            try {
                var compiledHandlebars = _handlebarsCompiler.Compile(markup);
                var rendered = compiledHandlebars(model);
            
                return rendered;
            } catch (HandlebarsRuntimeException ex) {
                _logger.LogError(ex, "Failed to render markup {Markup} with model {Model}", markup, model);

                throw;
            }
        }
    }
}
