using System.Text;
using DocDB.Configuration;

namespace DocDB.Services;

public class DocsService : IDocsService
{
    public DocsService(DatabaseConfiguration config)
    {
        docsDir = config.DocsDir;
    }

    private string docsDir;

    public async Task Set(string id, string content, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(docsDir, id);
        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
    }

    public async Task<string> Get(string id, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(docsDir, id);
        return await File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
    }
}

