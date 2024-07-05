using System;

namespace FinanceManager.Data.Utils;

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}