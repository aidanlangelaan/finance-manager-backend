namespace FinanceManager.Data.Extensions;

public static class ObjectExtensions
{
    public static bool HasMethod(this object objectToCheck, string methodName)
    {
        var type = objectToCheck.GetType();
        return type.GetMethod(methodName) != null;
    }

    public static bool HasProperty(this object objectToCheck, string propertyName)
    {
        var type = objectToCheck.GetType();
        return type.GetProperty(propertyName) != null;
    }

    public static T? GetPropertyValue<T>(this object objectToCheck, string propertyName)
    {
        var type = objectToCheck.GetType();
        var property = type.GetProperty(propertyName);

        if (property != null)
        {
            return (T)property.GetValue(objectToCheck, null)!;
        }

        return default(T);
    }
}