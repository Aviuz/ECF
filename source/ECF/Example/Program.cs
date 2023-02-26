using Autofac;
using Microsoft.Extensions.Configuration;

namespace ECF.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            new ECFConsoleProgram()
                .Configure((ctx, services) =>
                {
                    ctx.Intro = $"This is example console application based on ECF v.{typeof(Program).Assembly.GetName().Version}\nType help to list available commands";
                    ctx.Prefix = ">";

                    services.RegisterInstance(CreateConfiguration());
                    services.RegisterCommands<CommandAttribute>();
                })
                .Start(args);
        }

        static IConfiguration CreateConfiguration() => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }
}
