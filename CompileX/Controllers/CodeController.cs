using Microsoft.AspNetCore.Mvc;
using CompileX.Models;
using CompileX.Services;

namespace CompileX.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CodeController : ControllerBase
{
    private readonly CodeExecutionService _service;

    public CodeController(CodeExecutionService service)
    {
        _service = service;
    }

    [HttpPost("run")]
    public async Task<IActionResult> Run([FromBody] CodeRequest request)
    {
        var result = await _service.ExecuteAsync(request);
        return Ok(result);
    }
}
