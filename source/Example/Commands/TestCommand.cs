using ECF;
using ECF.Engine;

namespace Example.Commands
{
    [Command("list-test")]
    internal class TestCommand : CommandBase
    {
        private readonly CommandCollection collection;

        public TestCommand(CommandCollection collection)
        {
            this.collection = collection;
        }

        public override void Execute()
        {
            Console.WriteLine($"Types: {string.Join(", ", collection.GetAllCommands())}");
        }
    }
}
