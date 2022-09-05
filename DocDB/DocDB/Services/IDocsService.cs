namespace DocDB.Services;

public interface IDocsService
{
    Task Set(string id, string document, CancellationToken cancellationToken = default);
    Task<string> Get(string id, CancellationToken cancellationToken = default);
}

