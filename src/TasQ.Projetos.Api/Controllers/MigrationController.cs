using Microsoft.AspNetCore.Mvc;
using TasQ.Projetos.Data;

namespace TasQ.Projetos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MigrationController : ControllerBase
{
    private readonly MigrationService _migrationService;
    private readonly IConfiguration _config;

    public MigrationController(MigrationService migrationService, IConfiguration config)
    {
        _migrationService = migrationService;
        _config = config;
    }

    [HttpPost("ExecutarMigration")]
    public async Task<IActionResult> ExecutarMigration()
    {
        try
        {
            await _migrationService.ExecutarMigrationAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro: {ex.Message}");
        }
    }
}
