using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json.Linq;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing {
    public class LookupParser : DataTypeParser<INamedLookup>, ILookupParser {
        private readonly ILookups _lookups;

        public LookupParser(ILookups lookups) {
            _lookups = lookups;
        }
        
        public override bool CanParse(DataType dataType) {
            return dataType == OurDataTypes.Lookup;
        }

        public override ParseResult<INamedLookup> Parse(string text, Type targetType) {
            var parseResult = this.CallMethod(nameof(Parse))
                                  .OfGenericType(targetType)
                                  .WithParameter(typeof(string), text)
                                  .Run<IParseResult>();

            if (parseResult.Success) {
                return ParseResult.Success((INamedLookup) parseResult.Value);
            } else {
                return ParseResult.Fail<INamedLookup>();
            }
        }

        public ParseResult<TLookup> Parse<TLookup>(string text)
            where TLookup : class, INamedLookup {
            TLookup value = null;
            
            if (text.HasValue()) {
                text = text.Trim();

                var allLookups = _lookups.GetAll<TLookup>();

                foreach (var lookup in allLookups) {
                    var textValues = lookup.GetTextValues();

                    if (textValues.Contains(text, true)) {
                        value = lookup;
                        
                        break;
                    }
                }

                if (value == null) {
                    return ParseResult.Fail<TLookup>();
                }
            }

            return ParseResult.Success(value);
        }

        public ParseResult<TLookup> Parse<TLookup>(JToken token)
            where TLookup : class, INamedLookup {
            if (token.Type == JTokenType.String) {
                return Parse<TLookup>((string) token);
            } else {
                return ParseResult.Fail<TLookup>();
            }
        }
    }
}