using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ContractCheck.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DummyDataController : ControllerBase
{
    [HttpGet("excel")]
    public IActionResult GenerateDummyExcel()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Pegawai");

            // Add header
            worksheet.Cells[1, 1].Value = "KodePegawai";
            worksheet.Cells[1, 2].Value = "NamaPegawai";
            worksheet.Cells[1, 3].Value = "KodeCabang";
            worksheet.Cells[1, 4].Value = "KodeJabatan";
            worksheet.Cells[1, 5].Value = "TglMulaiKontrak";
            worksheet.Cells[1, 6].Value = "TglHabisKontrak";

            // Add data
            worksheet.Cells[2, 1].Value = "P004";
            worksheet.Cells[2, 2].Value = "Doni";
            worksheet.Cells[2, 3].Value = "C02";
            worksheet.Cells[2, 4].Value = "J01";
            worksheet.Cells[2, 5].Value = "2024-02-01";
            worksheet.Cells[2, 6].Value = "2025-02-01";

            worksheet.Cells[3, 1].Value = "P005";
            worksheet.Cells[3, 2].Value = "Eka";
            worksheet.Cells[3, 3].Value = "C03";
            worksheet.Cells[3, 4].Value = "J02";
            worksheet.Cells[3, 5].Value = "2023-07-01";
            worksheet.Cells[3, 6].Value = "2024-07-01";

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string excelName = "dummy_pegawai.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
