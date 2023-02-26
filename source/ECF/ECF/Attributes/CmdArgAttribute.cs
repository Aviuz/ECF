using System;

namespace ECF
{
    /// <summary>
    /// Command Flag
    /// 
    /// Register case-sensitive flag users can, but do not have to specify. Flags do not contain value. For example -p (short version), --myFlag (long version)
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CmdArgAttribute : Attribute
    {
        public string Key { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CmdArgAttribute(string key, int index)
        {
            Key = key;
            Index = index;
        }
    }
}
