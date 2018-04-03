using System;
using System.Collections.Generic;
using AutofacSample.Services;

namespace AutofacSample.Services.Fakes
{
    public class DynamicValuesService : IValuesService
    {
        public IEnumerable<string> GetValues()
        {
            return new string[] { "fake value1", "fake value2" };
        }
    }
}
