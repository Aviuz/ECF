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

# Input binding
You can bind input values, while using `CommandBase` and `AsyncCommandBase`, by using `Flag`/`Parameter`/`Argument` attributes.
Also you can add `[Required]` attribute to stop command from executing, if this specified input is not provided.
Types of properties inside command class can be any primitive type you want (internally it will be casted from string using `System.Convert` class) **with exception of Flags**. For Flags properties need to be of type `bool`.

## Flags
Flags can be enabled by adding specified token (ex. `-f`, `--flag`). If flag is specified in input, then proeprty will be changed to `true`, otherwise it will remain `false`.

### Example usage:
```bash
> command --silent
```
```csharp
[Flag("-s", "--silent", Description = "If specified, command will be run in silent mode.")]
public bool SilentMode { get; set; }
```

## Parameters
Parameters are input values that are passed after specified token (ex. `-p`, `--parameter`). It is good for optional input with string or number types, however you can specify it along with `[Required]` attribute if you want.

### Example usage:
```bash
> command --name "John Doe"
```
```csharp
[Parameter("-n", "--name", Description = "Specify your name.")]
public string? Name { get; set; }
```
## Arguments
Arguments are input values that are passed in order, so every Argument need to have order specified. Arguments are perfect for required input values. 
You can also specify `Name` to replace number in automatic syntax (ex. `command <0> <1>` => `command <source_file_path> <destination_file_path>`).

### Example usage:
```bash
> command C:\source C:\destination
```
```csharp
[Required, Argument(0, Name = "source_file_path", Description = "file path to file that will be copied")]
public string SourceFilePath { get; set; }

[Argument(1, Name = "destination_file_path", Description = "destination file path that the file will be copied to, if not specified it will be copied to clipboard")]
public string? DestinationFilePath { get; set; }
```
## Hosted services
You can start [hosted services](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services) in background, by installing additional package:
```shell
dotnet add package EasyConsoleFramework.HostedServices
```
and invoke AddHostedServices on `ECFHostBuilder`
```csharp
// Program.cs

await new ECFHostBuilder()
    // (...)
    .AddHostedServices() 
```

## User exceptions
You can mark exception type with `[UserException]` attribute to print user-friendly message, instead of standard exception string with stack trace. Additionally you can exit code and console color (of error message).

```csharp
[UserException(ExitCode = 2, ErrorTextColor = ConsoleColor.Yellow)]
public class DatabaseOfflineException() 
  : Exception("Database is offline, please try later.") { }
```

```bash
> connect --database "my-database"

Database is offline, please try later.
```

# Examples
For some other use cases please look into [Example Project](/source/Example).

# Advanced scenarios
For more advanced scenarios please refer to [this section](/docs/AdvancedScenarios.md).
