using DocDB.Models;

namespace DocDB.Services;

public interface IDocsService
{
    Task Set(string id, dynamic document, CancellationToken cancellationToken = default);
    Task<dynamic?> GetDocumentById(string id, CancellationToken cancellationToken = default);
    Task<dynamic[]> Search(Query query, CancellationToken cancellationToken = default);
}

