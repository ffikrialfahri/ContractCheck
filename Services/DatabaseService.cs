using ContractCheck.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ContractCheck.Services;

public class DatabaseService
{
    private readonly string? _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Pegawai>> GetEmployeeListAsync(string? kodePegawai, string? namaPegawai, string? kodeCabang, string? kodeJabatan)
    {
        var employees = new List<Pegawai>();
        if (string.IsNullOrEmpty(_connectionString)) return employees;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("sp_GetEmployeeList", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@KodePegawai", (object?)kodePegawai ?? DBNull.Value);
                command.Parameters.AddWithValue("@NamaPegawai", (object?)namaPegawai ?? DBNull.Value);
                command.Parameters.AddWithValue("@KodeCabang", (object?)kodeCabang ?? DBNull.Value);
                command.Parameters.AddWithValue("@KodeJabatan", (object?)kodeJabatan ?? DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new Pegawai
                        {
                            KodePegawai = reader["KodePegawai"].ToString(),
                            NamaPegawai = reader["NamaPegawai"].ToString(),
                            KodeCabang = reader["KodeCabang"].ToString(),
                            NamaCabang = reader["NamaCabang"].ToString(),
                            KodeJabatan = reader["KodeJabatan"].ToString(),
                            NamaJabatan = reader["NamaJabatan"].ToString(),
                            TglMulaiKontrak = Convert.ToDateTime(reader["TglMulaiKontrak"]),
                            TglHabisKontrak = Convert.ToDateTime(reader["TglHabisKontrak"])
                        });
                    }
                }
            }
        }
        return employees;
    }

    public async Task UpsertPegawaiAsync(Pegawai pegawai)
    {
        if (string.IsNullOrEmpty(_connectionString)) return;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var sql = @"
                MERGE Pegawai AS target
                USING (SELECT @KodePegawai AS KodePegawai) AS source
                ON (target.KodePegawai = source.KodePegawai)
                WHEN MATCHED THEN
                    UPDATE SET
                        NamaPegawai = @NamaPegawai, KodeCabang = @KodeCabang, KodeJabatan = @KodeJabatan,
                        TglMulaiKontrak = @TglMulaiKontrak, TglHabisKontrak = @TglHabisKontrak
                WHEN NOT MATCHED THEN
                    INSERT (KodePegawai, NamaPegawai, KodeCabang, KodeJabatan, TglMulaiKontrak, TglHabisKontrak)
                    VALUES (@KodePegawai, @NamaPegawai, @KodeCabang, @KodeJabatan, @TglMulaiKontrak, @TglHabisKontrak);";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@KodePegawai", (object?)pegawai.KodePegawai ?? DBNull.Value);
                command.Parameters.AddWithValue("@NamaPegawai", (object?)pegawai.NamaPegawai ?? DBNull.Value);
                command.Parameters.AddWithValue("@KodeCabang", (object?)pegawai.KodeCabang ?? DBNull.Value);
                command.Parameters.AddWithValue("@KodeJabatan", (object?)pegawai.KodeJabatan ?? DBNull.Value);
                command.Parameters.AddWithValue("@TglMulaiKontrak", pegawai.TglMulaiKontrak);
                command.Parameters.AddWithValue("@TglHabisKontrak", pegawai.TglHabisKontrak);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<Cabang>> GetCabangListAsync()
    {
        var cabangList = new List<Cabang>();
        if (string.IsNullOrEmpty(_connectionString)) return cabangList;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("sp_GetCabangList", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cabangList.Add(new Cabang
                        {
                            KodeCabang = reader["KodeCabang"].ToString(),
                            NamaCabang = reader["NamaCabang"].ToString()
                        });
                    }
                }
            }
        }
        return cabangList;
    }

    public async Task<List<Jabatan>> GetJabatanListAsync()
    {
        var jabatanList = new List<Jabatan>();
        if (string.IsNullOrEmpty(_connectionString)) return jabatanList;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("sp_GetJabatanList", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        jabatanList.Add(new Jabatan
                        {
                            KodeJabatan = reader["KodeJabatan"].ToString(),
                            NamaJabatan = reader["NamaJabatan"].ToString()
                        });
                    }
                }
            }
        }
        return jabatanList;
    }
}