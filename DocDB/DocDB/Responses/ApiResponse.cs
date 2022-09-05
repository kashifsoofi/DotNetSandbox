using System;
namespace DocDB.Responses;

public class ApiResponse
{
    public ApiResponse(dynamic body)
    {
        Body = body;
        Status = "ok";
    }

    public dynamic Body { get; }
    public string Status { get; }
}

