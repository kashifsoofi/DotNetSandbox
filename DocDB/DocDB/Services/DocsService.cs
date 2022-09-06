using System.Text;
using System.Text.Json;
using DocDB.Configuration;
using DocDB.Models;

namespace DocDB.Services;

public class DocsService : IDocsService
{
    public DocsService(DatabaseConfiguration config)
    {
        docsDir = config.DocsDir;
    }

    private readonly string docsDir;

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
        var documents = new List<dynamic>();
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

        return documents.ToArray();
    }
}

