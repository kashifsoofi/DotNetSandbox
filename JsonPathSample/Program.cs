using System;
using Json.More;
using Json.Path;

namespace JsonPathSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // var input = "$.id";
            var input = "$.id";
            var path = JsonPath.Parse(input);

            var data = new { id = 1, name = "sample" }.ToJsonDocument().RootElement;
            var result = path.Evaluate(data);

            Console.WriteLine(result.Error);
            Console.WriteLine(result.Matches[0].Value);
        }
    }
}
