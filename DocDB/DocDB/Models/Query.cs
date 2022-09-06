using System;
using System.Text.Json;

namespace DocDB.Models;

public class Query
{
    public Query(QueryClause[] clauses)
    {
        Clauses = clauses;
    }

    public QueryClause[] Clauses { get; }

    public bool Match(dynamic document)
    {
        foreach (var clause in Clauses)
        {
            var value = GetPath(document, clause.Key);
            if (value is null)
            {
                return false;
            }

            // Handle equality
            if (clause.Op == "=")
            {
                var match = value?.ToString() == clause.Value;
                if (!match)
                {
                    return false;
                }

                continue;
            }

            // Handle <, >
            decimal right;
            if (!decimal.TryParse(clause.Value, out right))
            {
                return false;
            }

            decimal left;
            switch (value?.ValueKind)
            {
                case JsonValueKind.Number:
                    left = value.GetDeimal();
                    break;
                case JsonValueKind.String:
                    if (!decimal.TryParse(value.GetString(), out left))
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            if (clause.Op == ">")
            {
                if (left <= right)
                {
                    return false;
                }

                continue;
            }

            if (left >= right)
            {
                return false;
            }
        }

        return true;
    }

    private JsonElement? GetPath(dynamic document, string[] parts)
    {
        var segment = (JsonElement)document;
        foreach (var part in parts)
        {
            var keyValuePairs = GetDictionary(segment);
            if (keyValuePairs is not null && !keyValuePairs.ContainsKey(part))
            {
                return null;
            }

            segment = keyValuePairs?[part];
        }

        return segment;
    }

    private Dictionary<string, dynamic>? GetDictionary(JsonElement jsonElement)
    {
        return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonElement.GetRawText());
    }
}

