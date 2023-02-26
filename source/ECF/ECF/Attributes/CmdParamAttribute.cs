using System;

namespace ECF
{
    /// <summary>
    /// Command Parameter
    /// 
    /// Register case-sensitive parameter with value that users can, but do not have to enter. For example -p <value> (short version), --myParam <value> (long version)
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CmdParamAttribute : Attribute
    {
        public string Key { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }

        public CmdParamAttribute(string key)
        {
            Key = key;
        }
    }
}
