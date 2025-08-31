namespace ContractCheck.Models;

public class Pegawai
{
    public string? KodePegawai { get; set; }
    public string? NamaPegawai { get; set; }
    public string? KodeCabang { get; set; }
    public string? NamaCabang { get; set; }
    public string? KodeJabatan { get; set; }
    public string? NamaJabatan { get; set; }
    public DateTime TglMulaiKontrak { get; set; }
    public DateTime TglHabisKontrak { get; set; }
}
