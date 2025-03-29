namespace ECF;

/// <summary>
/// Exceptions marked with this attribute will be displayed to user without stack trace for better UX experience.
/// It also allows to set error code and text color.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class UserExceptionAttribute : Attribute
{
    public int ExitCode { get; set; } = 1;
    public ConsoleColor ErrorTextColor { get; set; } = ConsoleColor.Red;
}
