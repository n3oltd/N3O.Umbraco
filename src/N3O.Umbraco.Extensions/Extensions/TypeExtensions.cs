using Humanizer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Umbraco.Cms.Core;

namespace N3O.Umbraco.Extensions;

public static class ReflectionExtensions {
    public static IReadOnlyList<Type> ApplyAttributeOrdering(this IEnumerable<Type> source) {
        var orderedList = source.OrderBy(x => x.HasAttribute<OrderAttribute>() ? 0 : 1)
                                .ThenBy(x => x.GetCustomAttribute<OrderAttribute>()?.Order)
                                .ToList();

        return orderedList;
    }
    
    public static IReadOnlyList<T> ApplyAttributeOrdering<T>(this IEnumerable<T> source) {
        var orderedList = source.OrderBy(x => x.GetType().HasAttribute<OrderAttribute>() ? 0 : 1)
                                .ThenBy(x => x.GetType().GetCustomAttribute<OrderAttribute>()?.Order)
                                .ToList();

        return orderedList;
    }
    
    public static MethodCallBuilder CallMethod(this object target, string name) {
        return new MethodCallBuilder(target.GetType(), target, name);
    }

    public static MethodCallBuilder CallStaticMethod(this Type staticType, string name) {
        return new MethodCallBuilder(staticType, null, name);
    }
    
    public static TType CreateInstance<TType, TParameter1>(this Type type,
                                                           TParameter1 parameter1) {
        return CreateInstance<TType>(type, parameter1);
    }

    public static TType CreateInstance<TType, TParameter1, TParameter2>(this Type type,
                                                                        TParameter1 parameter1,
                                                                        TParameter2 parameter2) {
        return CreateInstance<TType>(type, parameter1, parameter2);
    }

    public static TType CreateInstance<TType, TParameter1, TParameter2, TParameter3>(this Type type,
                                                                                     TParameter1 parameter1,
                                                                                     TParameter2 parameter2,
                                                                                     TParameter3 parameter3) {
        return CreateInstance<TType>(type, parameter1, parameter2, parameter3);
    }

    public static TType CreateInstance<TType, TParameter1, TParameter2, TParameter3, TParameter4>(this Type type,
                                                                                                  TParameter1 parameter1,
                                                                                                  TParameter2 parameter2,
                                                                                                  TParameter3 parameter3,
                                                                                                  TParameter4 parameter4) {
        return CreateInstance<TType>(type, parameter1, parameter2, parameter3, parameter4);
    }

    public static TType CreateInstance<TType>(this Type type, params object[] args) {
        return (TType)Activator.CreateInstance(type, args);
    }

    public static TType CreateGenericInstance<TType, TParameter1>(this Type type,
                                                                  Type genericType,
                                                                  TParameter1 parameter1) {
        return CreateGenericInstance<TType>(type, genericType, parameter1);
    }

    public static TType CreateGenericInstance<TType, TParameter1, TParameter2>(this Type type,
                                                                               Type genericType,
                                                                               TParameter1 parameter1,
                                                                               TParameter2 parameter2) {
        return CreateGenericInstance<TType>(type, genericType, parameter1, parameter2);
    }

    public static TType CreateGenericInstance<TType, TParameter1, TParameter2, TParameter3>(this Type type,
                                                                                            Type genericType,
                                                                                            TParameter1 parameter1,
                                                                                            TParameter2 parameter2,
                                                                                            TParameter3 parameter3) {
        return CreateGenericInstance<TType>(type, genericType, parameter1, parameter2, parameter3);
    }

    public static TType CreateGenericInstance<TType, TParameter1, TParameter2, TParameter3, TParameter4>(this Type type,
                                                                                                         Type genericType,
                                                                                                         TParameter1 parameter1,
                                                                                                         TParameter2 parameter2,
                                                                                                         TParameter3 parameter3,
                                                                                                         TParameter4 parameter4) {
        return CreateGenericInstance<TType>(type, genericType, parameter1, parameter2, parameter3, parameter4);
    }

    public static TType CreateGenericInstance<TType>(this Type type, Type genericType, params object[] args) {
        return (TType)Activator.CreateInstance(type.MakeGenericType(genericType), args);
    }
    
    public static void EnsurePathIsNotNull<T>(this T obj, Expression<Func<T, object>> expression) {
        var memberExpression = ExpressionUtility.ToMemberExpression(expression);
        
        EnsureExists(memberExpression, obj, 1);
    }

    public static IEnumerable<Type> GetAllConcreteTypesInAssemblyImplementingInterface(this Assembly assembly,
                                                                                       Type interfaceType) {
        var cacheKey = nameof(GetAllConcreteTypesInAssemblyImplementingInterface) + assembly.FullName + interfaceType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            var allMatchingTypes = new List<Type>();

            var nonGenericMatchingTypes = assembly.GetTypes()
                                                  .Where(t => t.ImplementsInterface(interfaceType) &&
                                                              t.IsConcreteClass())
                                                  .ToList();

            allMatchingTypes.AddRange(nonGenericMatchingTypes);

            if (interfaceType.IsGenericType) {
                var genericInterfaceType = interfaceType.GetGenericTypeDefinition();

                var genericTypes = assembly.GetTypes()
                                           .Where(t => t.IsConcreteClass() &&
                                                       t.IsGenericType &&
                                                       t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType))
                                           .ToList();

                if (genericTypes.Any()) {
                    var typeArguments = interfaceType.GetGenericArguments();
                    var genericTypesWithParameters = genericTypes.Select(x => {
                                                                     try {
                                                                         return x.MakeGenericType(typeArguments);
                                                                         // Thrown when type constraints not met
                                                                     } catch (ArgumentException) {
                                                                         return null;
                                                                     }
                                                                 })
                                                                 .ExceptNull()
                                                                 .ToList();
                    allMatchingTypes.AddRange(genericTypesWithParameters);
                }
            }

            return allMatchingTypes.ToArray();
        });
    }

    public static IEnumerable<Type> GetAllConcreteTypesInAssemblyImplementingInterface(this Assembly assembly, Type interfaceType, Func<Type, bool> filter) {
        return assembly.GetAllConcreteTypesInAssemblyImplementingInterface(interfaceType)
                       .Where(filter)
                       .AsEnumerable();
    }

    public static PropertyInfo[] GetAllProperties(this Type type) {
        return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }
    
    public static Type GetCollectionType(this Type type) {
        if (type.IsArray) {
            return type.GetElementType();
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
            return type.GetGenericArguments()[0];
        }

        var genericParameters = type.GetParameterTypesForGenericInterface(typeof(IEnumerable<>));

        return genericParameters.SingleOrDefault() ?? type;
    }
    
    public static IEnumerable<FieldInfo> GetConstantOrStaticFields(this Type type) {
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).ToList();

        return fields;
    }

    public static IEnumerable<T> GetConstantOrStaticValues<T>(this Type type) where T : class {
        var fields = GetConstantOrStaticFields(type);

        foreach (var field in fields) {
            if (field.IsLiteral) {
                if (field.GetRawConstantValue() is T constantValue) {
                    yield return constantValue;
                }
            } else if (field.GetValue(null) is T staticValue) {
                yield return staticValue;
            }
        }
    }

    public static IEnumerable<Type> GetParameterTypesForGenericInterface(this Type type,
                                                                         Type genericInterfaceType) {
        var cacheKey = nameof(GetParameterTypesForGenericInterface) + type.AssemblyQualifiedName + genericInterfaceType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            while (type != null && type != typeof(object)) {
                Type matchingInterfaceType;

                if (type.IsInterface &&
                    type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericInterfaceType) {
                    matchingInterfaceType = type;
                } else {
                    matchingInterfaceType = type.GetInterfaces()
                                                .FirstOrDefault(x => x.IsGenericType &&
                                                                     x.GetGenericTypeDefinition() == genericInterfaceType);
                }

                if (matchingInterfaceType != null) {
                    return matchingInterfaceType.GetGenericArguments();
                }

                type = type.BaseType;
            }

            return Enumerable.Empty<Type>().ToArray();
        });
    }
    
    public static IReadOnlyList<Type> GetGenericParameterTypesForInheritedGenericClass(this Type type, Type genericClassType) {
        var cacheKey = nameof(GetGenericParameterTypesForInheritedGenericClass) + type.AssemblyQualifiedName + genericClassType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            while (type != null && type != typeof(object)) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericClassType) {
                    return type.GetGenericArguments();
                }

                type = type.BaseType;
            }

            return Enumerable.Empty<Type>().ToArray();
        });
    }

    public static IEnumerable<Type> GetParameterTypesForGenericClass(this Type type, Type genericClassType) {
        var cacheKey = nameof(GetParameterTypesForGenericClass) + type.AssemblyQualifiedName + genericClassType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            while (type != null && type != typeof(object)) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericClassType) {
                    return type.GetGenericArguments();
                }

                type = type.BaseType;
            }

            return Enumerable.Empty<Type>().ToArray();
        });
    }
    
    public static string GetFriendlyName(this Type type) {
        if (type == typeof(int)) {
            return "int";
        } else if (type == typeof(short)) {
            return "short";
        } else if (type == typeof(byte)) {
            return "byte";
        } else if (type == typeof(bool)) {
            return "bool";
        } else if (type == typeof(long)) {
            return "long";
        } else if (type == typeof(float)) {
            return "float";
        } else if (type == typeof(double)) {
            return "double";
        } else if (type == typeof(decimal)) {
            return "decimal";
        } else if (type == typeof(string)) {
            return "string";
        } else if (type.IsGenericType) {
            return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)) + ">";
        } else {
            return type.Name;
        }
    }

    public static PropertyInfo GetPropertyInfo(this object obj,
                                               string name,
                                               StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase) {
        var property = obj.GetType().GetAllProperties().FirstOrDefault(x => string.Equals(x.Name, name, stringComparison));

        return property;
    }
    
    public static PropertyInfo GetPropertyInfo<T1>(this Expression<Func<T1, object>> expr) {
        var memberExpression = ExpressionUtility.ToMemberExpression(expr);
        var propertyInfo = (PropertyInfo) memberExpression.Member;
        var subExpression = memberExpression.Expression;

        while (subExpression is MemberExpression) {
            memberExpression = (MemberExpression) subExpression;
            propertyInfo = (PropertyInfo) memberExpression.Member;
            subExpression = memberExpression.Expression;
        }

        return propertyInfo;
    }

    public static string GetPropertyPath<TModel>(this Expression<Func<TModel, object>> expr,
                                                 bool camelCase = false) {
        var asString = expr.ToString();
        var parameterName = expr.Parameters.First().Name;

        if (asString.Replace(" ", "").EqualsInvariant($"{parameterName}=>{parameterName}")) {
            return null;
        }

        var firstDelimiter = asString.IndexOf('.');

        var pathComponents = asString.Substring(firstDelimiter + 1).Split('.');

        if (camelCase) {
            pathComponents = pathComponents.Select(x => x.Camelize()).ToArray();
        }

        var result = string.Join('.', pathComponents);

        return result;
    }

    public static Action<T, TProperty> GetSetter<T, TProperty>(this Expression<Func<T, TProperty>> expr) {
        var memberExpression = ExpressionUtility.ToMemberExpression(expr);
        var property = (PropertyInfo) memberExpression.Member;

        return ReflectionUtilities.EmitPropertySetter<T, TProperty>(property);
    }

    public static Type GetValueTypeForNullableType(this Type type) {
        var cacheKey = nameof(GetValueTypeForNullableType) + type.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => type.GetParameterTypesForGenericClass(typeof(Nullable<>)).Single());
    }
    
    public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute {
        return type.GetCustomAttributes<TAttribute>().HasAny();
    }

    public static bool HasAttribute(this Type type, Type attributeType) {
        return type.GetCustomAttributes(attributeType).HasAny();
    }

    public static bool HasAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute {
        return assembly.GetCustomAttributes<TAttribute>().HasAny();
    }

    public static bool HasAttribute(this Assembly assembly, Type attributeType) {
        return assembly.GetCustomAttributes(attributeType).HasAny();
    }

    public static bool HasAttribute<TAttribute>(this MethodInfo methodInfo) where TAttribute : Attribute {
        return methodInfo.GetCustomAttributes<TAttribute>().HasAny();
    }

    public static bool HasAttribute(this MethodInfo methodInfo, Type attributeType) {
        return methodInfo.GetCustomAttributes(attributeType).HasAny();
    }

    public static bool HasParameterlessConstructor(this Type type) {
        var cacheKey = nameof(HasParameterlessConstructor) + type.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, type.GetConstructor(new Type[0]) != null);
    }
    
    public static bool HasProperty(this object obj,
                                   string name,
                                   StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase) {
        return obj.GetType().GetAllProperties().Any(x => string.Equals(x.Name, name, stringComparison));
    }

    public static bool IsConcreteClass(this Type type) {
        var cacheKey = nameof(IsConcreteClass) + type.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, type.IsClass && !type.IsAbstract);
    }

    public static bool ImplementsInterface<TInterface>(this Type type) {
        return ImplementsInterface(type, typeof(TInterface));
    }

    public static bool ImplementsInterface(this Type type, Type interfaceType) {
        if (!interfaceType.IsInterface || (interfaceType.IsGenericType && !interfaceType.IsConstructedGenericType)) {
            throw new Exception($"{interfaceType.FullName.Quote()} is either not an interface or is not a constructed generic type");
        }

        var cacheKey = nameof(ImplementsInterface) + type.AssemblyQualifiedName + interfaceType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, interfaceType.IsAssignableFrom(type));
    }

    public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType) {
        if (!genericInterfaceType.IsGenericType || !genericInterfaceType.IsInterface) {
            throw new Exception($"{genericInterfaceType.FullName.Quote()} is not a generic interface type");
        }

        if (genericInterfaceType.IsConstructedGenericType) {
            throw new Exception($"{genericInterfaceType.FullName.Quote()} is not an open generic interface type");
        }

        var cacheKey = nameof(ImplementsGenericInterface) + type.AssemblyQualifiedName + genericInterfaceType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            while (type != null && type != typeof(object)) {
                if (type.IsInterface) {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == genericInterfaceType) {
                        return true;
                    }
                } else if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType)) {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        });
    }

    public static bool InheritsGenericClass(this Type type, Type genericClassType) {
        var cacheKey = nameof(InheritsGenericClass) + type.AssemblyQualifiedName + genericClassType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            while (type != null && type != typeof(object)) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericClassType) {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        });
    }
    
    public static bool IsCollectionType(this Type type) {
        return (type.IsArray || type.ImplementsGenericInterface(typeof(IEnumerable<>))) && type != typeof(string);
    }

    public static bool IsLookup(this Type type) {
        var cacheKey = nameof(IsLookup) + type.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, type.ImplementsInterface<ILookup>());
    }
    
    public static bool IsNullableType(this Type type) {
        var cacheKey = nameof(IsNullableType) + type.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            if (type.IsGenericType) {
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        });
    }

    public static bool IsOfTypeOrNullableType<T>(this Type type) where T : struct {
        var cacheKey = nameof(IsOfTypeOrNullableType) + type.AssemblyQualifiedName + typeof(T).AssemblyQualifiedName;

        return GetOrAdd(cacheKey, type == typeof(T) || type == typeof(T?));
    }

    public static bool IsSystemType(this Type type) {
        return type.Namespace.StartsWith("System");
    }

    public static bool IsSubclassOfType(this Type type, Type otherType) {
        var cacheKey = nameof(IsSubclassOfType) + type.AssemblyQualifiedName + otherType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, type.IsSubclassOf(otherType));
    }

    public static bool IsSubclassOrSubInterfaceOfGenericType(this Type type, Type genericType) {
        var cacheKey = nameof(IsSubclassOrSubInterfaceOfGenericType) + type.AssemblyQualifiedName + genericType.AssemblyQualifiedName;

        return GetOrAdd(cacheKey, () => {
            if (!genericType.IsGenericType) {
                throw new Exception($"{genericType.FullName.Quote()} is not a generic type");
            }

            while (type != null && type != typeof(object)) {
                var currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (genericType == currentType) {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        });
    }
    
    public static void SetPropertyValue(this object obj, string name, object value, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase) {
        obj.GetPropertyInfo(name, stringComparison).SetValue(obj, value);
    }
    
    private static object EnsureExists(MemberExpression node, object target, int depth) {
        if (node.Expression is MemberExpression innerExpression) {
            target = EnsureExists(innerExpression, target, depth + 1);
        }

        if (depth > 1) {
            var propertyInfo = (PropertyInfo) node.Member;
            var propertyType = propertyInfo.PropertyType;
            var propertyValue = propertyInfo.GetValue(target) ?? Activator.CreateInstance(propertyType);
            
            propertyInfo.SetValue(target, propertyValue);

            return propertyValue;
        }

        return target;
    }

    private static MissingMethodException MethodNotFound(string methodName, Type type) {
        return new MissingMethodException(type.FullName, methodName);
    }

    private static T GetOrAdd<T>(string key, Func<T> getValue) {
        return GetOrAdd(key, getValue());
    }

    private static T GetOrAdd<T>(string key, T value) {
        return value;

        // TODO Resolve memory usage before enabling again
        // var cacheKey = typeof(T).Name + key;
        //
        // return Cache.GetOrCreate(cacheKey, c => {
        //     c.SlidingExpiration = TimeSpan.FromSeconds(30);
        //
        //     return value;
        // });
    }

    private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions {
        Clock = new SystemClock(),
        CompactionPercentage = 0.1,
        ExpirationScanFrequency = TimeSpan.FromMinutes(1)
    });

    public class MethodCallBuilder {
        private readonly Type _targetType;
        private readonly object _targetInstance;
        private readonly string _methodName;
        private readonly List<Type> _genericTypes = new();
        private readonly List<Type> _parameterTypes = new();
        private readonly List<object> _parameters = new();

        public MethodCallBuilder(Type targetType, object targetInstance, string methodName) {
            _targetType = targetType;
            _targetInstance = targetInstance;
            _methodName = methodName;
        }

        public MethodCallBuilder OfGenericType<T>() {
            return OfGenericType(typeof(T));
        }

        public MethodCallBuilder OfGenericType(Type type) {
            _genericTypes.Add(type);

            return this;
        }

        public MethodCallBuilder WithParameter(Type type, object parameter) {
            _parameterTypes.Add(type);
            _parameters.Add(parameter);

            return this;
        }

        public Task RunAsync() {
            return (Task)Run();
        }

        public Task<T> RunAsync<T>() {
            return (Task<T>)RunAsync();
        }

        public T Run<T>() {
            return (T)Run();
        }

        public object Run() {
            var method = FindMethod(_targetType, _methodName, _parameterTypes, _genericTypes);

            if (method is null) {
                throw MethodNotFound(_methodName, _targetType);
            }

            return method.Invoke(_targetInstance, _parameters.ToArray());
        }

        private static MethodInfo FindMethod(Type classType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<Type> genericTypes) {
            var methodSignature = string.Concat(classType.AssemblyQualifiedName, methodName, string.Join('.', parameterTypes.Select(x => x.AssemblyQualifiedName)), string.Join('.', genericTypes.Select(x => x.AssemblyQualifiedName)));
            var cacheKey = nameof(FindMethod) + methodSignature;

            return GetOrAdd(cacheKey, () => {
                var genericParameterTypes = parameterTypes.Select(p => p.IsGenericType ? p.GetGenericTypeDefinition() : p).ToList();
                var numberOfGenerics = genericTypes.Count;
                var candidateMethods = new List<MethodInfo>();
            
                var type = classType;
                while (type != null && type != typeof(object)) {
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
                    var methodsMatchingNameAndParameterCount = allMethods.Where(m => m.Name.EqualsInvariant(methodName) &&
                                                                                     m.GetParameters().Length == parameterTypes.Count)
                                                                         .ToList();
                    
                    candidateMethods.AddRange(methodsMatchingNameAndParameterCount.Where(m => numberOfGenerics == 0 ||m.IsGenericMethod && m.GetGenericArguments().Length == numberOfGenerics).AsEnumerable());
                    
                    type = type.BaseType;
                }

                foreach (var method in candidateMethods) {
                    var concreteMethod = genericTypes.Any() ? method.MakeGenericMethod(genericTypes.ToArray()) : method;

                    var methodParameterTypes = concreteMethod.GetParameters().Select(p => p.ParameterType.IsGenericType ? p.ParameterType.GetGenericTypeDefinition() : p.ParameterType).ToList();

                    if (methodParameterTypes.SequenceEqual(genericParameterTypes)) {
                        return concreteMethod;
                    }
                }

                return null;
            });
        }
    }
}
