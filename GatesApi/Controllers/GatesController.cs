using GatesApi.Models;
using GatesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GatesApi.Controllers;

[ApiController]
[Route("api/gates")]
public class GatesController : ControllerBase
{
    private readonly IAccessCodeValidator _validator;

    public GatesController(IAccessCodeValidator validator)
    {
        _validator = validator;
    }

    [HttpPost("validate")]
    [EndpointSummary("Проверка кода доступа")]
    [ProducesResponseType(typeof(GateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GateResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Validate([FromBody] AccessCodeRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.AccessCode))
            return BadRequest(new GateResponse(
                false,
                new List<string> { "Код доступа отсутствует или пуст." },
                "Код отклонён."));

        return Ok(_validator.Validate(request.AccessCode));
    }
}
