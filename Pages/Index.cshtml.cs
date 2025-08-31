using ContractCheck.Models;
using ContractCheck.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContractCheck.Pages;

public class IndexModel : PageModel
{
    private readonly DatabaseService _databaseService;

    public IndexModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        PegawaiList = new List<Pegawai>();
    }

    public List<Pegawai> PegawaiList { get; set; }

    public async Task OnGetAsync()
    {
        PegawaiList = await _databaseService.GetEmployeeListAsync(null, null, null, null);
    }

    public IActionResult OnPostUpload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected");
        }

        // Here you would implement the logic to read the excel file
        // and perform CRUD operations.
        // For now, we'll just redirect to the index page.

        return RedirectToPage("/Index");
    }
}
