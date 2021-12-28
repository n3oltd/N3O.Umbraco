using System;

namespace N3O.Umbraco.Templates.Handlebars.Extensions {
    public static class HandlebarsArgumentsExtensions {
        public static bool TryGet<TArg, TValue>(this HandlebarsArguments args,
                                                int index,
                                                Func<TArg, TValue> convert,
                                                out TValue value) {
            if (args.TryGet<TArg>(index, out var arg)) {
                value = convert(arg);

                return true;
            } else {
                value = default;

                return false;
            }
        }
    }
}
