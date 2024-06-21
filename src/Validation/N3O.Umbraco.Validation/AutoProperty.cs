using N3O.Umbraco.Extensions;
using System;
using System.Linq;

namespace N3O.Umbraco.Validation;

[NoValidation]
public class AutoProperty<TProperty> where TProperty : class {
    private readonly PropertyFinder _propertyFinder;
    private readonly Func<TProperty, bool> _predicate;

    public AutoProperty(object obj, Func<TProperty, bool> predicate) {
        _propertyFinder = new PropertyFinder(obj);
        _predicate = predicate;
    }

    public TProperty Value {
        get {
            // This approach ensures if we have:
            // AutoProperty<Req>.Get(this, x => x?.Type == Type);
            // and Type has not been set that multiple properties will match (all of 
            // them as everything is null) and we proceed gracefully rather than
            // crashing if we used SingleOrDefault();
            var properties = _propertyFinder.FindProperties(_predicate).ToList();

            if (properties.IsSingle()) {
                return properties.Single();
            }

            return null;
        }
    }

    public static AutoProperty<TProperty> Get(object obj, Func<TProperty, bool> predicate) {
        return new AutoProperty<TProperty>(obj, predicate);
    }
}