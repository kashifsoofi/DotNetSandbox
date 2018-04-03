using System;
using System.Collections.Generic;

namespace AutofacSample.Services
{
    public interface IValuesService
    {
        IEnumerable<string> GetValues();
    }

    public class ValuesService : IValuesService
    {
        public IEnumerable<string> GetValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
