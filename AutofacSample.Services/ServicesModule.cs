using Autofac;

namespace AutofacSample.Services
{
    public class ServicesModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<ValuesService>().As<IValuesService>();
		}
	}
}
