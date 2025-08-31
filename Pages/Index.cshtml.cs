using ContractCheck.Models;
using ContractCheck.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;

namespace ContractCheck.Pages;

public class IndexModel : PageModel
{
    private readonly DatabaseService _databaseService;

    public IndexModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        PegawaiList = new List<Pegawai>();
        CabangList = new List<Cabang>();
        JabatanList = new List<Jabatan>();
        NewPegawai = new Pegawai();
    }

    public List<Pegawai> PegawaiList { get; set; }
    public List<Cabang> CabangList { get; set; }
    public List<Jabatan> JabatanList { get; set; }

    [BindProperty]
    public Pegawai NewPegawai { get; set; }

    public async Task OnGetAsync()
    {
        PegawaiList = await _databaseService.GetEmployeeListAsync(null, null, null, null);
        CabangList = await _databaseService.GetCabangListAsync();
        JabatanList = await _databaseService.GetJabatanListAsync();
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostUploadAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "File tidak dipilih atau file kosong.";
            return RedirectToPage("/Index");
        }

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var kodePegawai = worksheet.Cells[row, 1].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(kodePegawai)) continue;

                            var pegawai = new Pegawai
                            {
                                KodePegawai = kodePegawai,
                                NamaPegawai = worksheet.Cells[row, 2].Value?.ToString(),
                                KodeCabang = worksheet.Cells[row, 3].Value?.ToString(),
                                KodeJabatan = worksheet.Cells[row, 4].Value?.ToString(),
                                TglMulaiKontrak = Convert.ToDateTime(worksheet.Cells[row, 5].Value),
                                TglHabisKontrak = Convert.ToDateTime(worksheet.Cells[row, 6].Value)
                            };
                            await _databaseService.UpsertPegawaiAsync(pegawai);
                        }
                    }
                }
            }
            TempData["SuccessMessage"] = "File berhasil diunggah dan data berhasil diproses.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Terjadi kesalahan saat mengunggah file: {ex.Message}";
        }
        return RedirectToPage("/Index");
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            // Jika validasi gagal, muat kembali data yang diperlukan untuk halaman
            await OnGetAsync();
            return Page();
        }

        try
        {
            await _databaseService.UpsertPegawaiAsync(NewPegawai);
            TempData["SuccessMessage"] = "Data pegawai berhasil ditambahkan.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Gagal menyimpan data: {ex.Message}";
        }
        return RedirectToPage("/Index");
    }
}