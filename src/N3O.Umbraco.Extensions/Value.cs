using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco;

public abstract class Value : IEquatable<Value> {
    public static bool operator ==(Value lhs, Value rhs) {
        if (ReferenceEquals(lhs, rhs)) {
            return true;
        }

        if (ReferenceEquals(lhs, null)) {
            return false;
        }

        if (ReferenceEquals(rhs, null)) {
            return false;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Value lhs, Value rhs) {
        return !(lhs == rhs);
    }

    protected virtual IEnumerable<object> GetAtomicValues() {
        var ourProperties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(x => !x.HasAttribute<JsonIgnoreAttribute>() &&
                                                 !x.HasAttribute<ValueIgnoreAttribute>())
                                     .ToList();

        foreach (var property in ourProperties) {
            var propertyValue = property.GetValue(this);

            yield return propertyValue;
        }
    }

    public override bool Equals(object obj) {
        return Equals(obj as Value);
    }

    public bool Equals(Value other) {
        if (other == null) {
            return false;
        }

        using (var thisValues = GetAtomicValues().GetEnumerator()) {
            using (var otherValues = other.GetAtomicValues().GetEnumerator()) {
                while (thisValues.MoveNext() && otherValues.MoveNext()) {
                    if (ReferenceEquals(thisValues.Current, null) && !ReferenceEquals(otherValues.Current, null)) {
                        return false;
                    }

                    if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current)) {
                        return false;
                    }
                }

                return !thisValues.MoveNext() && !otherValues.MoveNext();
            }
        }
    }

    public override int GetHashCode() {
        var hashCode = GenerateHashCode();

        return hashCode.ToHashCode();
    }

    private HashCode GenerateHashCode() {
        var hashCode = new HashCode();

        foreach (var value in GetAtomicValues()) {
            if (value == null) {
                continue;
            }

            if (value is IEnumerable enumerable) {
                foreach (var item in enumerable) {
                    if (item == null) {
                        continue;
                    }
                    
                    hashCode.Add(item);
                }
            } else {
                hashCode.Add(value);
            }
        }

        return hashCode;
    }
}
