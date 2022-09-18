namespace DocDB.Responses;

public class SearchDocumentsResponse
{
    public SearchDocumentsResponse(
        dynamic[] documents)
    {
        Documents = documents;
    }

    public dynamic[] Documents { get; }
    public int Count => Documents.Length;
}

