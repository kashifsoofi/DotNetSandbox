using System.Text;
using System.Text.Json;
using DocDB.Configuration;
using DocDB.Extensions;
using DocDB.Models;

namespace DocDB.Services;

public class DocsService : IDocsService
{
    public DocsService(DatabaseConfiguration config)
    {
        docsDir = config.DocsDir;
        indexDir = config.IndexDir;
    }

    private readonly string docsDir;
    private readonly string indexDir;

    public async Task Set(string id, dynamic document, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(docsDir, id);
        var content = JsonSerializer.Serialize(document);
        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
    }

    public async Task<dynamic?> GetDocumentById(string id, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(docsDir, id);
        var content = await File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
        return JsonSerializer.Deserialize<dynamic>(content);
    }

    public async Task<dynamic[]> Search(Query query, CancellationToken cancellationToken = default)
    {
        var isRange = false;
        var idsArgumentCount = new Dictionary<string, int>();
        var nonRangeArguments = 0;
        foreach (var clause in query.Clauses)
        {
            if (clause.Op == "=")
            {
                nonRangeArguments++;

                var ids = await Lookup($"{clause.Key}.{clause.Value}");
                foreach (var id in ids)
                {
                    if (!idsArgumentCount.ContainsKey(id))
                    {
                        idsArgumentCount[id] = 0;
                    }

                    idsArgumentCount[id]++;
                }
            }
            else
            {
                isRange = true;
            }
        }

        var idsInAll = new List<string>();
        if (!query.SkipIndex)
        {
            foreach (KeyValuePair<string, int> kv in idsArgumentCount)
            {
                if (kv.Value == nonRangeArguments)
                {
                    idsInAll.Add(kv.Key);
                }
            }
        }

        var documents = new List<dynamic>();
        if (idsInAll.Count > 0)
        {
            foreach (var id in idsInAll)
            {
                var document = await GetDocumentById(id);
                if (!isRange || query.Match(document))
                {
                    documents.Add(new
                    {
                        Id = id,
                        Body = document,
                    });
                }
            }
        }
        else
        {
            var files = Directory.GetFiles(docsDir, "", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var document = await GetDocumentById(Path.GetFileName(file));
                if (query.Match(document))
                {
                    documents.Add(new
                    {
                        Id = file,
                        Body = document,
                    });
                }
            }
        }

        return documents.ToArray();
    }

    public async Task ReIndex()
    {
        var files = Directory.GetFiles(docsDir, "", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            var id = Path.GetFileName(file);
            var document = await GetDocumentById(id);
            await Index(id, document);
        }
    }

    public async Task Index(string id, dynamic document, CancellationToken cancellationToken = default)
    {
        var pathValues = GetPathValues(document, "");
        foreach (var pathValue in pathValues)
        {
            var idsString = "";
            var path = Path.Combine(indexDir, pathValue);
            if (File.Exists(path))
            {
                idsString = await File.ReadAllTextAsync(path, cancellationToken);
            }

            var ids = idsString.Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (!ids.Contains(id))
            {
                if (idsString.Length > 0)
                {
                    idsString += ",";
                }
                idsString = id;
            }

            await File.WriteAllTextAsync(path, idsString, cancellationToken);
        }
    }

    private string[] GetPathValues(dynamic document, string prefix)
    {
        var pathValues = new List<string>();

        var segment = (JsonElement)document;
        var keyValuePairs = segment.ToDictionary();
        foreach (KeyValuePair<string, dynamic> entry in keyValuePairs)
        {
            var key = entry.Key;
            var value = (JsonElement)entry.Value;
            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    pathValues.AddRange(GetPathValues(entry.Value, entry.Key));
                    continue;
                case JsonValueKind.Array:
                    // Can't handle arrays
                    continue;
            }

            if (prefix != "")
            {
                key = $"{prefix}.{key}";
            }
            pathValues.Add($"{key}={value.ToString()}");
        }

        return pathValues.ToArray();
    }

    private async Task<string[]> Lookup(string pathValue)
    {
        var path = Path.Combine(indexDir, pathValue);
        if (!File.Exists(path))
        {
            return new string[] { };
        }

        var idsString = await File.ReadAllTextAsync(path);
        return idsString.Split(",", StringSplitOptions.RemoveEmptyEntries);
    }
}

