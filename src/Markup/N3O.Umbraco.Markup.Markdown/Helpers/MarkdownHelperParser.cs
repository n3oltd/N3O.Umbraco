using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Markup.Markdown.Helpers {
    public class MarkdownHelperParser<T> : InlineParser where T : HelperArgs, new() {
        private readonly IReadOnlyList<string> _keywords;
        private readonly Action<IReadOnlyList<string>, T> _populateHelperArgs;

        public MarkdownHelperParser(IEnumerable<string> keywords,
                                    Action<IReadOnlyList<string>, T> populateHelperArgs) {
            _keywords = keywords.ToList();
            _populateHelperArgs = populateHelperArgs;

            OpeningCharacters = new[] { '{' };
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice) {
            if (!slice.PeekCharExtra(-1).IsWhiteSpaceOrZero() || slice.PeekChar(1) != '{') {
                return false;
            }

            var chars = new List<char>();

            while (!slice.CurrentChar.IsAnyOf('}', '\0')) {
                chars.Add(slice.CurrentChar);
                slice.NextChar();
            }

            if (slice.CurrentChar == '}' && slice.PeekCharExtra(1) == '}') {
                chars.Add(slice.CurrentChar);
                slice.NextChar();
                chars.Add(slice.CurrentChar);
                slice.NextChar();
            } else {
                return false;
            }

            var str = string.Concat(chars)
                            .Replace("{ ", "")
                            .Replace("{", "")
                            .Replace(" }", "")
                            .Replace("}", "");
            
            str = Regex.Replace(str, @"\s", " ");
            
            // https://stackoverflow.com/a/4780801
            var args = Regex.Split(str, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();

            if (args.None() || _keywords.None(x => x.EqualsInvariant(args[0]))) {
                return false;
            }
            
            var inlineStart = processor.GetSourcePosition(slice.Start, out var line, out var column);

            var inline = new T {
                Span = new SourceSpan {
                    Start = inlineStart,
                    End = inlineStart + chars.Count
                },
                Line = line,
                Column = column,
                Keyword = args[0]
            };
            
            try {
                _populateHelperArgs(args, inline);

                processor.Inline = inline;

                return true;
            } catch (Exception ex) {
                throw new Exception($"[{line}:{column}] Error processing helper {args[0]}: {ex.Message}");
            }
        }
    }
}