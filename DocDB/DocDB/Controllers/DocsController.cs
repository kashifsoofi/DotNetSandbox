using System.Text.Json;
using DocDB.Requests;
using DocDB.Responses;
using DocDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocDB.Controllers;

[ApiController]
[Route("[controller]")]
public class DocsController : ControllerBase
{
    private readonly IDocsService _docsService;
    private readonly IQueryParser _queryParser;
    private readonly ILogger<DocsController> _logger;

    public DocsController(
        IDocsService docsService,
        IQueryParser queryParser,
        ILogger<DocsController> logger)
    {
        _docsService = docsService;
        _queryParser = queryParser;
        _logger = logger;
    }

    [HttpPost(Name = "CreateDocument")]
    public async Task<IActionResult> Post([FromBody] dynamic document, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString();
        await _docsService.Set(id, document, cancellationToken);

        return Ok(new ApiResponse(new CreateDocumentResponse(id)));
    }

    [HttpGet("{id}", Name = "GetDocumentById")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == default)
        {
            throw new ArgumentException("Guid value cannot be default", nameof(id));
        }

        var document = await _docsService.GetDocumentById(id.ToString(), cancellationToken);        
        return Ok(new ApiResponse(new GetDocumentByIdResponse(document)));
    }

    [HttpGet(Name = "SearchDocuments")]
    public async Task<IActionResult> Get([FromQuery] SearchDocumentsRequest request, CancellationToken cancellationToken = default)
    {
        var query = _queryParser.Parse(request.Q);
        var documents = await _docsService.Search(query, cancellationToken);
        return Ok(new ApiResponse(new SearchDocumentsResponse(documents)));
    }
}

