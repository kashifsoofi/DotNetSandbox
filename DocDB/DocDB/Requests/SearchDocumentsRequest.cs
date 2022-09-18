using System;
namespace DocDB.Requests;

public class SearchDocumentsRequest
{
    public bool SkipIndex { get; set; }
    public string Q { get; set; }
}

