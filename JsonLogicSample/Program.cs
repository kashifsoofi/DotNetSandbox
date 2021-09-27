using System;
using System.Text.Json;
using Json.Logic;
using Json.More;

namespace JsonLogicSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonLogic = "{\"and\": [{\" == \": [{\"var\":\"id\"}, 1]}, {\" == \": [{\"var\":\"name\"}, \"sample\"]}]}";
            var rule = JsonSerializer.Deserialize<Rule>(jsonLogic);

            var data = new { id = 1, name = "sample" }.ToJsonDocument().RootElement;
            var result = rule.Apply(data);

            Console.WriteLine(result.ToJsonString());
        }
    }
}
