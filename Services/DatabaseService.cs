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

        if (string.IsNullOrEmpty(_connectionString))
        {
            return employees;
        }

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
                            KodePegawai = Convert.ToString(reader["KodePegawai"]),
                            NamaPegawai = Convert.ToString(reader["NamaPegawai"]),
                            KodeCabang = Convert.ToString(reader["KodeCabang"]),
                            NamaCabang = Convert.ToString(reader["NamaCabang"]),
                            KodeJabatan = Convert.ToString(reader["KodeJabatan"]),
                            NamaJabatan = Convert.ToString(reader["NamaJabatan"]),
                            TglMulaiKontrak = Convert.ToDateTime(reader["TglMulaiKontrak"]),
                            TglHabisKontrak = Convert.ToDateTime(reader["TglHabisKontrak"])
                        });
                    }
                }
            }
        }

        return employees;
    }
}
