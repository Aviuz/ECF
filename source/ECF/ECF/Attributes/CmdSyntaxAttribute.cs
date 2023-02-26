using System;

namespace ECF
{
    /// <summary>
    /// Command Syntax
    /// 
    /// Add syntax to command, so it will give hint how to use command.
    /// 
    /// Note: If used on class that not implement BaseCommand it has no effects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CmdSyntaxAttribute : Attribute
    {
        public string SyntaxExpression { get; set; }

        public CmdSyntaxAttribute(string syntaxExpression)
        {
            SyntaxExpression = syntaxExpression;
        }
    }
}
