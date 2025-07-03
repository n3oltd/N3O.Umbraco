using Microsoft.AspNetCore.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Extensions;

public static class ObjectExtensions {
    public static T GetNonPublicProperty<T>(this object o, string propertyName) {
        var properties = o.GetType()
                          .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                          .ToList();
    
        foreach (var property in properties) {
            if (property.Name == propertyName) {
                return (T) property.GetValue(o);
            }
        }

        throw new Exception($"Property {propertyName} not found");
    }

    public static bool HasAny<T1, T2>(this T1 obj, Func<T1, IEnumerable<T2>> selector) {
        if (obj == null) {
            return false;
        }

        var collection = selector?.Invoke(obj);
    
        return collection.HasAny();
    }

    public static bool HasValue<T>(this T obj, Func<T, object> selector) {
        if (obj == null) {
            return false;
        }

        var item = selector?.Invoke(obj);

        return HasValue(item);
    }

    public static bool HasValue(this object obj) {
        var hasValue = false;

        if (obj == null) {
            hasValue = false;
        } else if (obj is string str) {
            hasValue = str.HasValue();
        } else if (obj is HtmlString htmlString) {
            hasValue = htmlString.HasValue();
        } else if (obj is ICollection collection) {
            foreach (var element in collection) {
                if (HasValue(element)) {
                    hasValue = true;
                    break;
                }
            }
        } else {
            hasValue = true;
        }

        return hasValue;
    }

    public static TResult IfNotNull<TValue, TIntermediate, TResult>(this TValue value,
                                                                    Func<TValue, TIntermediate> selector,
                                                                    Func<TIntermediate, TResult> func)
        where TValue : class
        where TIntermediate : class {
        if (value == null) {
            return default;
        }

        var intermediate = selector(value);

        return IfNotNull(intermediate, func);
    }

    public static TResult IfNotNull<TValue, TIntermediate, TResult>(this TValue? value,
                                                                    Func<TValue, TIntermediate> selector,
                                                                    Func<TIntermediate, TResult> func)
        where TValue : struct
        where TIntermediate : class {
        if (value == null) {
            return default;
        }

        var intermediate = selector(value.Value);

        return IfNotNull(intermediate, func);
    }

    public static TResult IfNotNull<TValue, TResult>(this TValue value, Func<TValue, TResult> func)
        where TValue : class {
        if (value == null) {
            return default;
        }

        var result = func(value);

        return result;
    }

    public static TResult IfNotNull<TValue, TResult>(this TValue? value, Func<TValue, TResult> func)
        where TValue : struct {
        if (value == null) {
            return default;
        }

        var result = func(value.Value);

        return result;
    }

    public static IEnumerable<T2> OrEmpty<T1, T2>(this T1 obj, Func<T1, IEnumerable<T2>> selector) {
        if (obj == null) {
            return [];
        }

        var collection = selector?.Invoke(obj) ?? Enumerable.Empty<T2>();
    
        return collection;
    }
    
    public static object ResolvePathValue(this object data, string path, char pathSeparator = '.') {
        var pathComponents = path.Split(pathSeparator);

        var obj = data;
        
        foreach (var pathComponent in pathComponents) {
            if (obj == null) {
                break;
            }
            
            var propertyInfo = obj.GetType().GetProperties().SingleOrDefault(x => x.Name.EqualsInvariant(pathComponent));

            obj = propertyInfo?.GetValue(obj);
        }

        return obj;
    }
}
