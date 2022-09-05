namespace DocDB.Responses;

public class CreateDocumentResponse
{
    public CreateDocumentResponse(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

