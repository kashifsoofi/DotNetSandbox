namespace Communication.Api.Tests.Acceptance
{
    using System;
    using TechTalk.SpecFlow;

    [Binding]
    public class CustomTransforms
    {
        [StepArgumentTransformation]
        public bool BooleanTransformation(string s)
        {
            return Convert.ToBoolean(s);
        }
    }
}
