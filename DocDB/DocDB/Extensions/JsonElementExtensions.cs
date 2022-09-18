using System;
using System.Text.Json;

namespace DocDB.Extensions;

public static class JsonElementExtensions
{
    public static Dictionary<string, dynamic>? ToDictionary(this JsonElement jsonElement)
    {
        return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonElement.GetRawText());
    }
}

