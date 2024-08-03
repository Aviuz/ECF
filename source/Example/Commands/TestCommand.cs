using ECF;
using ECF.Engine;

namespace Example.Commands;

[Command("list-test")]
internal class TestCommand : CommandBase
{
    private readonly ICommandCollection collection;

    public TestCommand(ICommandCollection collection)
    {
        this.collection = collection;
    }

    public override void Execute()
    {
        Console.WriteLine($"Types: {string.Join(", ", collection.GetAllCommands())}");
    }
}
