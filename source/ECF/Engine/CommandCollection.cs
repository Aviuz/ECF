using ECF.Exceptions;

namespace ECF.Engine;

/// <summary>
/// This class is responsible for mapping command names to actual command types.
/// </summary>
public interface ICommandCollection
{
    IEnumerable<Type> GetAllCommands();
    Type? GetCommand(string? binding);
    void Register(IEnumerable<string> commandAliases, Type commandType);
}

/// <inheritdoc cref="ICommandCollection" />
public class CommandCollection : ICommandCollection
{
    private Dictionary<string, Type> commandBindings = new Dictionary<string, Type>();

    public Type? GetCommand(string? binding)
    {
        binding = binding?.ToUpper();

        if (string.IsNullOrWhiteSpace(binding) || !commandBindings.ContainsKey(binding))
            return null;

        return commandBindings[binding];
    }

    public IEnumerable<Type> GetAllCommands() => commandBindings.Values.Distinct();

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
}
