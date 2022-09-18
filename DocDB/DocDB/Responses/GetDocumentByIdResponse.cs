namespace DocDB.Responses;

public class GetDocumentByIdResponse
{
    public GetDocumentByIdResponse(
        dynamic document)
    {
        Document = document;
    }

    public dynamic Document { get; }
}

