namespace Communication.Api.Tests.Acceptance.Steps
{
    using System.Threading.Tasks;
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

        public MessagesSteps(ScenarioContext context, CommunicationApiClient communicationApiClient)
        {
            this.context = context;
            this.communicationApiClient = communicationApiClient;
        }

        [Given(@"following message values")]
        public void GivenFollowingMessageValues(Table table)
        {
            var request = table.CreateInstance<MessageRequest>();
            context.Set(request, "Request");
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
    }
}
