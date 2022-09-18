using System;
namespace DocDB.Models;

public class QueryClause
{
    public QueryClause(string[] key, string value, string op)
    {
        Key = key;
        Value = value;
        Op = op;
    }

    public string[] Key { get; }
    public string Value { get; }
    public string Op { get; }
}

