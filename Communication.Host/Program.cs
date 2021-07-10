namespace Communication.Host
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Communication.Host.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var host = CreateHostBuilder(args)
                .UseSerilog()
                .ConfigureContainer((HostBuilderContext hostBuilderContext, ContainerBuilder builder) =>
                {
                    var startup = new Startup(hostBuilderContext.Configuration);
                    startup.ConfigureContainer(builder);
                })
                .UseConsoleLifetime();

            await host.Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
