using Autofac;
using Autofac.Extensions.DependencyInjection;
using DocDB.Configuration;
using DocDB.Services;

var builder = WebApplication.CreateBuilder(args);

var configurationManager = builder.Configuration;

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    var databaseConfiguration = configurationManager.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();
    builder.RegisterInstance(databaseConfiguration).AsSelf().SingleInstance();

    builder.RegisterType<DocsService>().As<IDocsService>().SingleInstance();
    builder.RegisterType<QueryParser>().As<IQueryParser>().SingleInstance();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var docsService = scope.ServiceProvider.GetRequiredService<IDocsService>();
    docsService.ReIndex();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
