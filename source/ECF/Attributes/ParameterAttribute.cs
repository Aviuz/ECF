namespace ECF
{
    /// <summary>
    /// Command Parameter
    /// 
    /// Register case-sensitive parameter with value that users can, but do not have to enter. For example -p <value> (short version), --myParam <value> (long version)
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ParameterAttribute : Attribute
    {
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public string? Description { get; set; }

        public ParameterAttribute() { }
    }
}
