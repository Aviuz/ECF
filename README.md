# Easy Console Framework [![NuGet](https://img.shields.io/nuget/v/ECF.svg?label=ECF)](https://nuget.org/packages/ECF) [![NuGet](https://img.shields.io/nuget/v/ECFTemplates.svg?label=ECFTemplates)](https://nuget.org/packages/ECFTemplates)
.NET Core library for easy building console application with command line parsing and inversion of control (IoC).  
  
By default it's using `Microsoft.Extensions.DependencyInjection`.  
To use [Autofac](https://autofac.org) please refer to [Using Autofac](#Using-Autofac).  
To use this with custom IoC please refer to [Using custom IoC](#using-custom-IoC)

It was designed for easy building application with multiple commands and low coupling.

Currently only works on Windows due to `shell32.dll` dependency.

For some other use cases please look into [Example Project](/source/Example).

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
```
This is example console application based on ECF. Version 0.1.0.
Type help to list available commands
```
5. Invoke your command in console by typing 
```
> hello-world -n John
```

# Template
You can use ECF template to create new projects. Firstly you need to install template:
```
dotnet new install ECFTemplates
```

Then you can create new projects using 
```
dotnet new ecf -o MyNewProject
```


# Custom commands
By default it will initialize all commands inside current and entry assembly with `[Command]` attribute. To change that you can register commands by calling `RegisterCommands(params Assembly[])` method:
```cs
.Configure((ctx, services, registry) =>
{
    // ...
    registry.RegisterCommands<MyCommandAttribute>(typeof(Class).Assembly);
})
```

To remove default commands such as `help`, `exit`, and `load-script` you need to remove this line:
~~`.UseDefaultCommands()`~~

and register commands manually:
```cs
.Configure((ctx, services, registry) =>
{
    // ...
    registry.RegisterCommands<ECF.CommandAttribute>(System.Reflection.Assembly.GetExecutingAssembly());
})
```

# Using Autofac
To use ECF with [Autofac](https://autofac.org) install [EasyConsoleFramwork.Autofac](https://nuget.org/packages/EasyConsoleFramework.AutoFac) package.
Startup code look very similar with only namespace change:
```cs
// Program.cs
using ECF.Autofac;

await new ECFHostBuilder()
    .UseDefaultCommands() // register all commands with CommandAttribute and default commands (help, exit, ...)
    .AddConfiguration(optional: true) // adds appsettings.json        
    .Configure((ctx, containerBuilder, _) =>
    {
        ctx.Intro = $"This is example console application based on ECF. Version {typeof(Program).Assembly.GetName().Version}.\nType help to list available commands";
        ctx.HelpIntro = "Welcome to example program that showcases ECF framework. Enter one of command listed below";
        ctx.Prefix = "> ";
    })
    .RunAsync(args);
```

# Using custom IoC
To configure ECF with custom IoC follow this steps:
1. Install nuget package `EasyConsoleFramwork.Base`
2. Implement IoC adapters
    - `IIoCBuilderAdapter` - adapter for interacting with IoC when building (similar to `IServiceCollection`)
    - `IIoCProviderAdapter` - adapter for resolving dependencies (similar to `IServiceProvider`)
    - `IIoCScopeAdapter` - adapter for scopes, it's implementing `IDisposable` interface to dispose scope after it ends
3. Create `ECFHostBuilderBase` with your `IIoCBuilderAdapter` implementation
4. Other steps are similar to normal ECF application

Then you can create new projects using 
```
dotnet new ecf -o MyNewProject
```