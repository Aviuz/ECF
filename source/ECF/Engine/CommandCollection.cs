using ECF.Engine.Exceptions;

namespace ECF.Engine;

/// <summary>
/// This class is responsible for mapping command names to actual command types.
/// 
/// 
/// A string-type dictionary wrapper for mapping command name to actual command type.
/// 
/// </summary>
public class CommandCollection
{
    private Dictionary<string, Type> commandBindings = new Dictionary<string, Type>();

    public Type? DefaultCommand { get; private set; }

    public Type? GetCommand(string? binding)
    {
        binding = binding?.ToUpper();
        if (string.IsNullOrWhiteSpace(binding) || !commandBindings.ContainsKey(binding))
            return DefaultCommand;

        return commandBindings[binding];
    }

    public IEnumerable<Type> GetAllCommands()
    {
        IEnumerable<Type> commandsWithBindings = commandBindings.Values;

        if (DefaultCommand != null)
            commandsWithBindings = commandsWithBindings.Append(DefaultCommand);

        return commandsWithBindings.Distinct();
    }

    public void Register(IEnumerable<string> commandAliases, Type commandType)
    {
        if (commandType.IsAssignableTo(typeof(ICommand)) == false)
            throw new MissingCommandInterfaceException(commandType);

        var transformedAliases = commandAliases
            .Where(alias => string.IsNullOrWhiteSpace(alias) == false)
            .Select(alias => alias.ToUpper());

        foreach (string alias in transformedAliases)
            commandBindings[alias] = commandType;
    }

    public void RegisterDefaultCommand<T>() where T : ICommand
    {
        DefaultCommand = typeof(T);
    }
}
