using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Extensions {
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
            where TResult : class
            where TIntermediate : class {
            if (value == null) {
                return null;
            }

            var intermediate = selector(value);

            return IfNotNull(intermediate, func);
        }
    
        public static TResult IfNotNull<TValue, TIntermediate, TResult>(this TValue? value,
                                                                        Func<TValue, TIntermediate> selector,
                                                                        Func<TIntermediate, TResult> func)
            where TValue : struct
            where TResult : class
            where TIntermediate : class {
            if (value == null) {
                return null;
            }

            var intermediate = selector(value.Value);

            return IfNotNull(intermediate, func);
        }
    
        public static TResult IfNotNull<TValue, TResult>(this TValue value, Func<TValue, TResult> func)
            where TValue : class
            where TResult : class {
            if (value == null) {
                return null;
            }

            var result = func(value);

            return result;
        }
    
        public static TResult IfNotNull<TValue, TResult>(this TValue? value, Func<TValue, TResult> func)
            where TValue : struct
            where TResult : class {
            if (value == null) {
                return null;
            }

            var result = func(value.Value);

            return result;
        }
    
        public static IEnumerable<T2> OrEmpty<T1, T2>(this T1 obj, Func<T1, IEnumerable<T2>> selector) {
            if (obj == null) {
                return Enumerable.Empty<T2>();
            }

            var collection = selector?.Invoke(obj) ?? Enumerable.Empty<T2>();
        
            return collection;
        }

        public static JToken ToJToken(this object obj, IJsonProvider jsonProvider) {
            if (obj == null) {
                return JValue.CreateNull();
            }

            if (obj.GetType().IsNullableType()) {
                obj = obj.GetPropertyValue(nameof(Nullable<int>.Value));
            }
            
            if (obj is string @string) {
                return new JValue(@string);
            } else if (obj is bool @bool) {
                return new JValue(@bool);
            } else if (obj is DateTime dateTime) {
                return new JValue(dateTime);
            } else if (obj is DateTimeOffset dateTimeOffset) {
                return new JValue(dateTimeOffset);
            } else if (obj is Guid guid) {
                return new JValue(guid);
            } else if (obj is int @int) {
                return new JValue(@int);
            } else if (obj is long @long) {
                return new JValue(@long);
            } else if (obj is double @double) {
                return new JValue(@double);
            } else if (obj is decimal @decimal) {
                return new JValue(@decimal);
            } else if (obj is float @float) {
                return new JValue(@float);
            } else {
                return jsonProvider.SerializeObject(obj);
            }
        }
    }
}
