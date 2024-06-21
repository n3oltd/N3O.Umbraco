using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Validation;

public class PropertyFinder {
    private readonly object _object;

    public PropertyFinder(object obj) {
        _object = obj;
    }

    public IEnumerable<TProperty> FindProperties<TProperty>(Func<TProperty, bool> predicate = null)
        where TProperty : class {
        var properties = _object.GetType()
                                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p => typeof(TProperty).IsAssignableFrom(p.PropertyType))
                                .Select(s => (TProperty) s.GetValue(_object))
                                .Where(p => predicate?.Invoke(p) ?? true)
                                .ToList();

        return properties;
    }

    public PropertyInfo FindPropertyInfo<TProperty>(Func<PropertyInfo, bool> predicate = null) where TProperty : class {
        var property = _object.GetType()
                              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                              .Where(p => typeof(TProperty).IsAssignableFrom(p.PropertyType))
                              .Single(p => predicate?.Invoke(p) ?? true);

        return property;
    }
}