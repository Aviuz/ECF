# Easy Console Framework
.NET Core library for easy building console application with command line parsing and inversion of control (IoC) powered by [Autofac](https://autofac.org).

It was designed for easy building application with multiple commands and low coupling.

Currently only works on Windows due to `shell32.dll` dependency.

# How to use
1. Put in your *program.cs* this fragment:
```cs
// Program.cs
using ECF;

new ECFHostBuilder()
    .UseDefaultCommands() // register all commands with CommandAttribute and default commands (help, exit, ...)
    .AddConfiguration(optional: true) // adds appsettings.json        
    .Configure((ctx, services, _) =>
    {
        ctx.Intro = $"This is example console application based on ECF. Version {typeof(Program).Assembly.GetName().Version}.\nType help to list available commands";
        ctx.HelpIntro = "Welcome to example program that showcases ECF framework. Enter one of command listed below";
        ctx.Prefix = ">";
    })
    .Run(args);
```
it will initialize and run your ECF console application

2. You can now add your first command
```cs
[Command("hello-world")]
class HelloWorldCommand : CommandBase
{
    private readonly IConfiguration configuration;

    [Parameter(ShortName = "n", LongName = "name", Description = "Your name")]
    public string Name { get; set; }

    public ReadSettingsCommand(IConfiguration configuration)
    {
        // you can use constructor to inject services
        this.configuration = configuration;
    }

    public override void Execute()
    {
        if (Name == null)
        {
            DisplayHelp();
            return;
        }

        Console.WriteLine($"Hello {Name}");
    }
}
```
3. Run your program
you should see welcome info
```
This is example console application based on ECF. Version 0.1.0.
Type help to list available commands
```
4. Invoke your command in console by typing 
```
> hello-world -n John
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
