namespace ECF.Utilities;

internal static class ExConverter
{
    public static object? ChangeType(object? value, Type conversionType)
    {
        var underlyingType = Nullable.GetUnderlyingType(conversionType);
        if (underlyingType != null)
        {
            if (value == null)
                return null;

            return Convert.ChangeType(value, underlyingType);
        }

        return Convert.ChangeType(value, conversionType);
    }
}
