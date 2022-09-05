using System.Text.Json;
using DocDB.Responses;
using DocDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocDB.Controllers;

[ApiController]
[Route("[controller]")]
public class DocsController : ControllerBase
{
    private readonly IDocsService _docsService;
    private readonly ILogger<DocsController> _logger;

    public DocsController(
        IDocsService docsService,
        ILogger<DocsController> logger)
    {
        _docsService = docsService;
        _logger = logger;
    }

    [HttpPost(Name = "CreateDocument")]
    public async Task<IActionResult> Post([FromBody] dynamic document, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString();
        var content = JsonSerializer.Serialize(document);
        await _docsService.Set(id, content, cancellationToken);

        return Ok(new ApiResponse(new CreateDocumentResponse(id)));
    }

    [HttpGet("{id}", Name = "GetDocumentById")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == default)
        {
            throw new ArgumentException("Guid value cannot be default", nameof(id));
        }

        var content = await _docsService.Get(id.ToString(), cancellationToken);
        var document = JsonSerializer.Deserialize<dynamic>(content);
        return Ok(new ApiResponse(new GetDocumentByIdResponse(document)));
    }
}

