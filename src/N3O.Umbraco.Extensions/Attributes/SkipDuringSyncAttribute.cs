using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SkipDuringSyncAttribute : Attribute { }