using System.Reflection;

namespace ECF;

public static class InternalsExtensions
{
    public static MethodInfo AsyncCommandBase_ApplyArguments = typeof(AsyncCommandBase).GetMethod("ApplyArguments", BindingFlags.NonPublic | BindingFlags.Instance)!;
    public static void ApplyArguments(this CommandBase command, CommandArguments args)
    {
        AsyncCommandBase_ApplyArguments.Invoke(command, [args]);
    }

    public static MethodInfo AsyncCommandBase_Validate = typeof(AsyncCommandBase).GetMethod("Validate", BindingFlags.NonPublic | BindingFlags.Instance)!;
    public static bool Validate(this CommandBase command, out IList<string> errorMessages)
    {
        object[] parameters = [new List<string>()];
        bool result = (bool)AsyncCommandBase_Validate.Invoke(command, parameters)!;
        errorMessages = (IList<string>)parameters[0];
        return result;
    }

    public static MethodInfo AsyncCommandBase_GetSyntaxExpression = typeof(AsyncCommandBase).GetMethod("GetSyntaxExpression", BindingFlags.NonPublic | BindingFlags.Instance)!;
    public static string? GetSyntaxExpression(this CommandBase command)
    {
        return (string?)AsyncCommandBase_GetSyntaxExpression.Invoke(command, []);
    }
}
