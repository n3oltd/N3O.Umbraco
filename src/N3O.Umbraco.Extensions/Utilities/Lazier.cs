using Microsoft.Extensions.DependencyInjection;
using System;

namespace N3O.Umbraco.Utilities;

public class Lazier<T> : Lazy<T> {
    public Lazier(IServiceScopeFactory serviceScopeFactory)
        : base(() => serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<T>()) { }
}
