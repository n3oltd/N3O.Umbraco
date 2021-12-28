using Microsoft.Extensions.DependencyInjection;
using System;

namespace N3O.Umbraco.Utilities {
    public class Lazier<T> : Lazy<T> {
        public Lazier(IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<T>) { }
    }
}
