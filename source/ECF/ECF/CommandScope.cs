using Autofac;
using ECF.Engine;

namespace ECF
{
    /// <summary>
    /// This is class wraping CommandProcessor. It is dedicated to provide different command sets inside one application.
    /// By default there is created one default CommandScope with registered commands with [Command] attribute
    /// </summary>
    public interface ICommandScope
    {
        ICommandProcesor Procesor { get; }
    }

    /// <inheritdoc cref="ECF.ICommandScope"/>
    public class CommandScope : ICommandScope
    {
        public ICommandProcesor Procesor { get; protected set; }

        public CommandScope(IContainer container)
        {
            Procesor = new CommandProcessor(container);
        }

        public CommandScope(ICommandProcesor commandProcesor)
        {
            Procesor = commandProcesor;
        }
    }
}
