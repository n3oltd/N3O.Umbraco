﻿using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Markup.Markdown.Helpers {
    public abstract class Helper : Helper<EmptyHelperArgs> {
        protected Helper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }
        
        protected Helper(IEnumerable<string> keywords, int minArgs, int maxArgs) : base(keywords, minArgs, maxArgs) { }

        protected override void Parse(IReadOnlyList<string> args, EmptyHelperArgs inline) { }
    }

    public abstract class Helper<T> : HtmlObjectRenderer<T>, IMarkdownExtension where T : HelperArgs, new() {
        private readonly int _minArgs;
        private readonly int _maxArgs;
        private readonly MarkdownHelperParser<T> _parser;

        protected Helper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }
        
        protected Helper(IEnumerable<string> keywords, int minArgs, int maxArgs) {
            _minArgs = minArgs;
            _maxArgs = maxArgs;
            _parser = new MarkdownHelperParser<T>(keywords, (args, x) => Parse(PreprocessArgs(args), x));
        }
        
        public void Setup(MarkdownPipelineBuilder pipeline) {
            pipeline.InlineParsers.AddIfNotAlready(_parser);
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
            var renderers = (renderer as HtmlRenderer)?.ObjectRenderers;

            if (renderers != null && !renderers.Contains(this)) {
                renderers.Insert(0, this);
            }
        }
        
        protected virtual IReadOnlyList<string> PreprocessArgs(IReadOnlyList<string> args) {
            var argsCount = args.Count - 1;
                
            if (argsCount < _minArgs || argsCount > _maxArgs) {
                throw new Exception("Incorrect number of parameters passed");
            }
            
            return args;
        }

        protected override void Write(HtmlRenderer renderer, T obj) {
            Render(renderer, obj);
        }

        protected abstract void Parse(IReadOnlyList<string> args, T inline);
        protected abstract void Render(HtmlRenderer renderer, T inline);
    }
}