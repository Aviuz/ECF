using ECF;
using Microsoft.Extensions.Configuration;

namespace Example.Commands
{
    [Command("read-setting")]
    internal class ReadSettingsCommand : CommandBase
    {
        private readonly IConfiguration configuration;

        public ReadSettingsCommand(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override void Execute()
        {
            Console.WriteLine($"MySetting value is: {configuration["MySetting"]}");
        }
    }
}
