using System;
using Json.More;
using Json.Schema;

namespace JsonSchemaSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new { id = 1, name = "sample" }.ToJsonDocument().RootElement;

            var jsonSchema = "{ \"proerties\": { \"id\": { \"const\": 1 } } }";
            var schema = JsonSchema.FromText(jsonSchema);
            var result = schema.Validate(data);
            Console.WriteLine(result.IsValid);

            var jsonSchema1 = "{ \"properties\": { \"id\": { \"const\":1}, \"name\": { \"const\":\"sample\"} } }";
            var schema1 = JsonSchema.FromText(jsonSchema1);
            result = schema1.Validate(data);
            Console.WriteLine(result.IsValid);
        }
    }
}
