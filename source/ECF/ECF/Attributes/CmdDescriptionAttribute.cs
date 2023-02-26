using System;

namespace ECF
{
    /// <summary>
    /// Command Syntax
    /// 
    /// Add description to command, so it will give some description what command do.
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CmdDescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public CmdDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
