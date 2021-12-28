using N3O.Umbraco.Mediator;
using System;

namespace N3O.Umbraco.Scheduler {
    public static class TriggerKey {
        private const string Separator = "|";

        public static string Generate<TRequest, TModel>() where TRequest : Request<TModel, None> {
            return Generate(typeof(TRequest), typeof(TModel));
        }

        public static string Generate(Type requestType, Type modelType) {
            return $"{requestType.AssemblyQualifiedName}{Separator}{modelType.AssemblyQualifiedName}";
        }

        public static Type ParseRequestType(string triggerId) {
            return Parse(triggerId).RequestType;
        }

        public static Type ParseModelType(string triggerId) {
            return Parse(triggerId).ModelType;
        }

        public static (Type RequestType, Type ModelType) Parse(string triggerKey) {
            var bits = triggerKey.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            var requestType = Type.GetType(bits[0]);
            var modelType = Type.GetType(bits[1]);

            return (requestType, modelType);
        }
    }
}