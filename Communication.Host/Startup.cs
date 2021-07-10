namespace Communication.Host
{
    using System;
    using Autofac;
    using Communication.Host.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TimedHostedService>().As<IHostedService>();
        }
    }
}
