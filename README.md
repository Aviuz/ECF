# Easy Console Framework [![NuGet](https://img.shields.io/nuget/v/ECF.svg?label=ECF&logo=nuget)](https://nuget.org/packages/ECF) [![NuGet](https://img.shields.io/nuget/v/ECFTemplates.svg?label=ECFTemplates&logo=nuget)](https://nuget.org/packages/ECFTemplates)
.NET Core library for easy building console application with command line parsing and inversion of control (IoC).  
By default it's using `Microsoft.Extensions.DependencyInjection` (see configuration for [AutoFac](/docs/AdvancedScenarios.md/#using-autofac), [custom](/docs/AdvancedScenarios.md/#using-custom-ioc)).  
It was designed for easy building application with multiple commands and low coupling.

# How to use
1. Install nuget package [ECF](https://nuget.org/packages/ECF)
2. Put in your *program.cs* this fragment:
```cs
// Program.cs
using ECF;

await new ECFHostBuilder()
    .UseDefaultCommands() // register all commands with CommandAttribute and default commands (help, exit, ...)
    .AddConfiguration(optional: true) // adds appsettings.json        
    .Configure((ctx, services, _) =>
    {
        ctx.Intro = $"This is example console application based on ECF. Version {typeof(Program).Assembly.GetName().Version}.\nType help to list available commands";
        ctx.HelpIntro = "Welcome to example program that showcases ECF framework. Enter one of command listed below";
        ctx.Prefix = "> ";
    })
    .RunAsync(args);
```
it will initialize and run your ECF console application

3. You can now add your first command
```cs
using ECF;

[Command("hello-world")]
class HelloWorldCommand : CommandBase
{
    private readonly IConfiguration configuration;

    [Required, Parameter("--name", "-n", Description = "Your name")]
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
```
4. Run your program
you should see welcome info
```shell
This is example console application based on ECF. Version 0.0.0.
Type help to list available commands
```
5. Invoke your command in console by typing 
```shell
> hello-world -n John
```

# Template
You can use ECF template to create new projects. Firstly you need to install template:
```shell
dotnet new install ECFTemplates
```

Then you can create new projects using 
```shell
dotnet new ecf -o MyNewProject
```

# Examples
For some other use cases please look into [Example Project](/source/Example).

# Advanced scenarios
For more advanced scenarios please refer to [this section](/docs/AdvancedScenarios.md).
