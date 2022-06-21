using N3O.Umbraco.Exceptions;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Parameters;

public abstract class NamedParameter<TValue> : INamedParameter<TValue>, INamedParameterFromString {
    public abstract string Name { get; }

    object INamedParameter.Value => Value;

    public TValue Value { get; set; }

    public virtual void FromString(string value) {
        var converter = TypeDescriptor.GetConverter(typeof(TValue));

        if (converter.CanConvertFrom(typeof(string))) {
            Value = (TValue) converter.ConvertFrom(value);
        }
    }

    public override string ToString() {
        return Value?.ToString();
    }

    public void Run(Action<TValue> action) {
        try {
            action(Value);
        } catch (ResourceNotFoundException) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }
    }

    public T Run<T>(Func<TValue, T> func, bool mandatory) {
        var result = func(Value);

        if (mandatory && result == null) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }

        return result;
    }

    public void Run(Func<TValue, bool> func, bool mandatory) {
        var result = func(Value);

        if (mandatory && result == false) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }
    }

    public async Task RunAsync(Func<TValue, Task> func) {
        try {
            await func(Value);
        } catch (ResourceNotFoundException) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }
    }

    public async Task<T> RunAsync<T>(Func<TValue, Task<T>> func, bool mandatory) {
        var result = await func(Value);

        if (mandatory && result == null) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }

        return result;
    }

    public async Task RunAsync(Func<TValue, Task<bool>> func, bool mandatory) {
        var result = await func(Value);

        if (mandatory && result == false) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }
    }

    public async Task RunAsync(Func<TValue, CancellationToken, Task> func,
                               CancellationToken cancellationToken) {
        try {
            await func(Value, cancellationToken);
        } catch (ResourceNotFoundException) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }
    }

    public async Task<T> RunAsync<T>(Func<TValue, CancellationToken, Task<T>> func,
                                     bool mandatory,
                                     CancellationToken cancellationToken) {
        var result = await func(Value, cancellationToken);

        if (mandatory && result == null) {
            throw new ResourceNotFoundException(Name, Value?.ToString());
        }

        return result;
    }

    public static implicit operator string(NamedParameter<TValue> namedParameter) {
        return namedParameter.Value?.ToString();
    }
}

internal static class NamedParameter {
    public static INamedParameter Create(Type type) {
        var namedParameter = (INamedParameter) Activator.CreateInstance(type);

        return namedParameter;
    }
}
