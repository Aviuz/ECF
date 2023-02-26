using Autofac;

namespace ECF.Utilities
{
    public class CommandDispatcher
    {
        private readonly ILifetimeScope lifetimeScope;

        public CommandDispatcher(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public void ExecuteCommand<T>(params string[] commandArgs) where T : ICommand
        {
            using (var nestedScope = lifetimeScope.BeginLifetimeScope())
            {
                var command = nestedScope.Resolve<T>();
                command.ApplyArguments(new CommandArguments() { Arguments = commandArgs });
                command.Execute();
            }
        }
    }
}
