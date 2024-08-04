# Custom command attributes
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

# Customizing ECF engine 
You can use ECF in some more advanced scenarios, for example:
    - using commands inside text area in another application
    - loading scripts, especially with different sets of commands
    - (if you find anything more let me know, I'll put it here)

Before we proceed we neet to understand basics of ECF engine:
  - `CommandProcessor` is heart of ECF, it's responsible for parsing single line input and executing commands
    - it uses `IIoCProviderAdapter` to get `ICommandResolver` and `InterfaceContext`
        - `ICommandResolver` is responsible for finding & creating commands by name
            - `CommandResolver` does this by using `ICommandCollection` to find commands, and `IIoCProviderAdapter` to create them
        - `InterfaceContext` is general-purpose object for holding some configuration data
    - `CommandProcessor` can be wrapped to bind with user input
  - `AsyncCommandBase` and `CommandBase` along with default commands are basekit commands, which really does not take part in engine itself
  - `ECFHostBuilder` is just adding some dependencies to IoC and creates `CommandLineInterface` with `CommandProcessor` inside.

So instructions for you to implement more advanced scenarios:
  - We have 2 components that wire input for you:
    - `CommandLineInterface` works as a interface between command based application and console window. If you want to bind input to console use this, otherwise use `ScriptLoader` or `CommandProcessor` instead.
    - `ScriptLoader` is intedended to execute commands inside script
  - If you want to provide input yourself you can just use `CommandProcessor`. 
  - Keep in mind that those components are heavily dependent on other IoC components.
    - For `Autofac`/`Microsoft.Extensions.DependencyInjection` you can use `services.AddECFCommandRegistry(context, cfg => (...)).BuildAndCreateECFCommandProcessor()`
    - Otherwise `CommandRegistryBuilder` should get you convered for all dependencies
       * You will need to implement `IIoCBuilderAdapter`/`IIoCProviderAdapter` yourself, or use ready adapters.
       * You need also to register singleton of `InterfaceContext`

