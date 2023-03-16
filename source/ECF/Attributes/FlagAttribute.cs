namespace ECF
{
    /// <summary>
    /// Command Argument
    /// 
    /// Register argument that users will enter in specific order. User cannot change order, or name arguments. For example <argument_1> <argument_2>
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FlagAttribute : Attribute
    {
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public string? Description { get; set; }

        public FlagAttribute() { }
    }
}
