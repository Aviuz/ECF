using ECF;
using Microsoft.Extensions.Configuration;

namespace NewProject;

[Command("hello-world")]
class HelloWorldCommand : CommandBase
{
    private readonly IConfiguration configuration;

    [Required, Parameter("-n", "--name", Description = "Your name")]
    public string Name { get; set; }

    public HelloWorldCommand(IConfiguration configuration)
    {
        // you can use constructor to inject services
        this.configuration = configuration;
    }

    public override void Execute()
    {
        Console.WriteLine($"Hello {Name}");
    }
}