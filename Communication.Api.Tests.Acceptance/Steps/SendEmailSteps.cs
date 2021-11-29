namespace Communication.Api.Tests.Acceptance.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Communication.Api.Tests.Acceptance.Helpers;
    using Communication.ApiClient;
    using Communication.Contracts.Requests;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class MessagesSteps
    {
        private readonly ScenarioContext context;
        private readonly CommunicationApiClient communicationApiClient;
        private readonly EmailHelper emailHelper;

        public MessagesSteps(ScenarioContext context, CommunicationApiClient communicationApiClient, EmailHelper emailHelper)
        {
            this.context = context;
            this.communicationApiClient = communicationApiClient;
            this.emailHelper = emailHelper;
        }

        [Given(@"following message values")]
        public void GivenFollowingMessageValues(Table table)
        {
            var request = table.CreateInstance<MessageRequest>();
            context.Set(request, "Request");
            context.Set("ToEmailAddress", request.To);
        }
        
        [When(@"the client posts the inputs to send endpoint")]
        public async Task WhenTheClientPostsTheInputsToSendEndpoint()
        {
            var request = context.Get<MessageRequest>("Request");
            var result = await communicationApiClient.SendAsync(request);
            context.Set(result, "Result");
        }
        
        [Then(@"the result should be (.*)")]
        public void ThenAnOkStatusShouldBeReturned(bool expectedResult)
        {
            var result = context.Get<bool>("Result");
            result.Should().Be(expectedResult);
        }

        [Then(@"client receives and Email with subject '(.*)'")]
        public async Task ThenCleintReceivesAndEmailWithSubject(string subject)
        {
            Thread.Sleep(TimeSpan.FromSeconds(60));
            var emailAddress = context.Get<string>("ToEmailAddress");
            await emailHelper.VerifyEmailAsync(emailAddress, subject);
        }

    }
}
