using Autofac;

namespace AutofacSample.Services.Fakes
{
    public class DynamicServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DynamicValuesService>().As<IValuesService>();
        }
    }
}
