namespace Communication.Api.Tests.Acceptance
{
    using BoDi;
    using Communication.Api.Tests.Acceptance.ApiClients;
    using Communication.Api.Tests.Acceptance.Helpers;
    using Communication.ApiClient;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer objectContainer;
        private readonly ScenarioContext scenarioContext;

        private static IConfigurationRoot configurationRoot;

        private static Mail7Configuration mail7Configuration = new Mail7Configuration();

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appSettingsFilePath = Path.Combine(basePath, "appSettings.json");

            File.Exists(appSettingsFilePath).Should().BeTrue("appSettings.json file not found");

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddJsonFile(appSettingsFilePath, false)
                .AddEnvironmentVariables();

            configurationRoot = configurationBuilder.Build();

            configurationRoot.GetSection(nameof(Mail7Configuration)).Bind(mail7Configuration);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            objectContainer.RegisterInstanceAs(new CommunicationApiClient(new HttpClient
            {
                BaseAddress = new Uri(configurationRoot.GetValue<string>("CommunicationApi:BaseUrl")),
            }));

            objectContainer.RegisterInstanceAs(
                new EmailHelper(new Mail7Client(mail7Configuration)));
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await objectContainer.Resolve<EmailHelper>().CleanUpTrackedEntities(default);
        }
    }
}
