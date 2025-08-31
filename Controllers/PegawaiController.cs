using ContractCheck.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractCheck.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PegawaiController : ControllerBase
{
    private readonly DatabaseService _databaseService;

    public PegawaiController(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? kodePegawai, [FromQuery] string? namaPegawai, [FromQuery] string? kodeCabang, [FromQuery] string? kodeJabatan)
    {
        var employees = await _databaseService.GetEmployeeListAsync(kodePegawai, namaPegawai, kodeCabang, kodeJabatan);
        return Ok(employees);
    }

    [HttpPost("upload")]
    public IActionResult Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected");
        }

        // Here you would implement the logic to read the excel file
        // and perform CRUD operations.
        // For now, we'll just return a success message.

        return Ok(new { message = "File uploaded successfully" });
    }
}
