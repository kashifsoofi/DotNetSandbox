namespace Communication.Api.Tests.Acceptance
{
    using BoDi;
    using Communication.ApiClient;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer objectContainer;
        private readonly ScenarioContext scenarioContext;

        private static IConfigurationRoot configurationRoot;

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
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            objectContainer.RegisterInstanceAs(new CommunicationApiClient(new HttpClient
            {
                BaseAddress = new Uri(configurationRoot.GetValue<string>("CommunicationApi:BaseUrl")),
            }));
        }
    }
}
