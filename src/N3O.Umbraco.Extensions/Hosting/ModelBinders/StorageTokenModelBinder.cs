using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting {
    public class StorageTokenModelBinder : IModelBinder {
        private readonly IJsonProvider _jsonProvider;

        public StorageTokenModelBinder(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext) {
            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None) {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var base64EncodedData = valueProviderResult.FirstValue;
            var json = Base64.Decode(base64EncodedData);

            if (!json.HasValue()) {
                return Task.CompletedTask;
            }

            try {
                var jsonSettings = _jsonProvider.GetSettings();
                var storageToken = JsonConvert.DeserializeObject<StorageToken>(json, jsonSettings);

                bindingContext.Result = ModelBindingResult.Success(storageToken);
            } catch {
                bindingContext.ModelState.TryAddModelError(modelName, $"{base64EncodedData.Quote()} is not a valid storage token");
            }

            return Task.CompletedTask;
        }
    }
    
    public class StorageTokenModelBinderProvider : IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (context.Metadata.ModelType == typeof(StorageToken)) {
                return new BinderTypeModelBinder(typeof(StorageTokenModelBinder));
            }

            return null;
        }
    }
}