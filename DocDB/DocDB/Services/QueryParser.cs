using System;
using System.Text;
using DocDB.Models;

namespace DocDB.Services;

public class QueryParser : IQueryParser
{
    public Query Parse(bool skipIndex, string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return new Query(skipIndex, new QueryClause[] { });
        }

        var clauses = new List<QueryClause>();
        for (var i = 0; i < q.Length; i++)
        {
            // Eat whitespace
            while (char.IsWhiteSpace(q[i]))
            {
                i++;
            }

            (var key, var nextIndex, var error) = GetToken(q, i);
            if (error != null)
            {
                throw new Exception($"Expected valid key, got [{error}], `{q.Substring(nextIndex)}`");
            }

            // Expect some operator
            if (q[nextIndex] != ':')
            {
                throw new Exception($"Expected colon at {nextIndex}, got: `{q[nextIndex]}`");
            }
            i = nextIndex + 1;

            var op = "=";
            if (q[i] == '>' || q[i] == '<')
            {
                op = q[i].ToString();
                i++;
            }

            (var value, nextIndex, error) = GetToken(q, i);
            if (error != null)
            {
                throw new Exception($"Expected valid value, got [{error}], `{q.Substring(nextIndex)}`");
            }
            i = nextIndex;

            var clause = new QueryClause(key.Split(".", StringSplitOptions.RemoveEmptyEntries), value, op);
            clauses.Add(clause);
        }

        return new Query(skipIndex, clauses.ToArray());
    }

    private (string, int, string?) GetToken(string input, int index)
    {
        if (index >= input.Length)
        {
            return ("", index, null);
        }

        var token = "";
        if (input[index] == '"')
        {
            index++;
            var foundEnd = false;

            while (index < input.Length)
            {
                if (input[index] == '"')
                {
                    foundEnd = true;
                    break;
                }

                token += input[index];
                index++;
            }

            if (!foundEnd)
            {
                return ("", index, "Expected end of quoted string");
            }

            return (token, index + 1, null);
        }

        // If unquoted, read as much contiguous digits/letters as there are
        while (index < input.Length)
        {
            if (!(char.IsLetterOrDigit(input[index]) || input[index] == '.'))
            {
                break;
            }

            token += input[index];
            index++;
        }

        if (token.Length == 0)
        {
            return ("", index, "No string found");
        }

        return (token, index, null);
    }
}

