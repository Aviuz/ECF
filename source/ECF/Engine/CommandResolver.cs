using ECF.Commands;
using ECF.InverseOfControl;

namespace ECF.Engine
{
    /// <summary>
    /// Command factory which is based on command string passed by user.
    /// </summary>
    public interface ICommandResolver
    {
        ICommand CreateCommand(CommandArguments args);
        ICommand? Resolve(string command);
        ICommand Resolve(Type commandType);
        IEnumerable<ICommandAttribute> GetAllCommands();
    }

    /// <inheritdoc cref="ICommandResolver" />
    public class CommandResolver : ICommandResolver
    {
        private readonly IIoCProviderAdapter iocProvider;
        private readonly CommandCollection collection;

        public CommandResolver(IIoCProviderAdapter iocProvider, CommandCollection collection)
        {
            this.iocProvider = iocProvider;
            this.collection = collection;
        }

        public ICommand CreateCommand(CommandArguments args)
        {
            ICommand? command = Resolve(args.CommandName);

            if (command == null)
                command = new NotFoundCommand();

            command.ApplyArguments(args);

            return command;
        }

        public ICommand? Resolve(string command)
        {
            var type = GetCommandType(command);

            if (type == null)
                return null;

            return Resolve(type);
        }

        public ICommand Resolve(Type commandType)
        {
            object resolvedObject = iocProvider.Resolve(commandType);

            if (resolvedObject is ICommand command)
                return command;
            else
                throw new Exception($"Command '{resolvedObject.GetType().FullName}' need to implement {nameof(ICommand)} interface");
        }

        public IEnumerable<ICommandAttribute> GetAllCommands() => collection.GetAllCommands();

        private Type? GetCommandType(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
                return null;

            return collection.GetCommand(commandName);
        }
    }
}
