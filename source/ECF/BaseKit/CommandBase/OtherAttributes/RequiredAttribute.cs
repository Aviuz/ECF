namespace ECF;

/// <summary>
/// Marks that property is required and must be supplied in comamnd line. Otherwise it will print error and syntax instead of execution.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class RequiredAttribute : Attribute
{
    /// <summary>
    /// If set it will display custom error message.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
