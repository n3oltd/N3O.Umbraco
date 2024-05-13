using System;

namespace N3O.Umbraco.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class NoValidationAttribute : Attribute { }