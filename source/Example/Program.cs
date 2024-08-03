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