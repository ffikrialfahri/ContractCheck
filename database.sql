
-- Create Cabang Table
CREATE TABLE Cabang (
    KodeCabang VARCHAR(10) PRIMARY KEY,
    NamaCabang VARCHAR(100) NOT NULL
);

-- Create Jabatan Table
CREATE TABLE Jabatan (
    KodeJabatan VARCHAR(10) PRIMARY KEY,
    NamaJabatan VARCHAR(100) NOT NULL
);

-- Create Pegawai Table
CREATE TABLE Pegawai (
    KodePegawai VARCHAR(10) PRIMARY KEY,
    NamaPegawai VARCHAR(100) NOT NULL,
    KodeCabang VARCHAR(10) FOREIGN KEY REFERENCES Cabang(KodeCabang),
    KodeJabatan VARCHAR(10) FOREIGN KEY REFERENCES Jabatan(KodeJabatan),
    TglMulaiKontrak DATE NOT NULL,
    TglHabisKontrak DATE NOT NULL
);

-- Insert some sample data for Cabang
INSERT INTO Cabang (KodeCabang, NamaCabang) VALUES
('C01', 'Jakarta'),
('C02', 'Bandung'),
('C03', 'Surabaya');

-- Insert some sample data for Jabatan
INSERT INTO Jabatan (KodeJabatan, NamaJabatan) VALUES
('J01', 'Software Engineer'),
('J02', 'Project Manager'),
('J03', 'UI/UX Designer');

-- Insert some sample data for Pegawai
INSERT INTO Pegawai (KodePegawai, NamaPegawai, KodeCabang, KodeJabatan, TglMulaiKontrak, TglHabisKontrak) VALUES
('P001', 'Budi', 'C01', 'J01', '2024-01-01', '2025-01-01'),
('P002', 'Ani', 'C02', 'J02', '2023-06-15', '2024-06-15'),
('P003', 'Cici', 'C01', 'J03', '2024-03-01', '2025-03-01');
GO

-- Stored Procedure to get employee list with dynamic filtering
CREATE PROCEDURE sp_GetEmployeeList
    @KodePegawai VARCHAR(10) = NULL,
    @NamaPegawai VARCHAR(100) = NULL,
    @KodeCabang VARCHAR(10) = NULL,
    @KodeJabatan VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @ParamList NVARCHAR(MAX);

    SET @SQL = N'
        SELECT
            p.KodePegawai,
            p.NamaPegawai,
            c.KodeCabang,
            c.NamaCabang,
            j.KodeJabatan,
            j.NamaJabatan,
            p.TglMulaiKontrak,
            p.TglHabisKontrak
        FROM
            Pegawai p
        INNER JOIN
            Cabang c ON p.KodeCabang = c.KodeCabang
        INNER JOIN
            Jabatan j ON p.KodeJabatan = j.KodeJabatan
        WHERE 1=1'

    IF @KodePegawai IS NOT NULL
        SET @SQL = @SQL + N' AND p.KodePegawai = @pKodePegawai';

    IF @NamaPegawai IS NOT NULL
        SET @SQL = @SQL + N' AND p.NamaPegawai LIKE ''%'' + @pNamaPegawai + ''%''';

    IF @KodeCabang IS NOT NULL
        SET @SQL = @SQL + N' AND p.KodeCabang = @pKodeCabang';

    IF @KodeJabatan IS NOT NULL
        SET @SQL = @SQL + N' AND p.KodeJabatan = @pKodeJabatan';

    SET @ParamList = N'
        @pKodePegawai VARCHAR(10),
        @pNamaPegawai VARCHAR(100),
        @pKodeCabang VARCHAR(10),
        @pKodeJabatan VARCHAR(10)';

    EXEC sp_executesql @SQL, @ParamList,
        @pKodePegawai = @KodePegawai,
        @pNamaPegawai = @NamaPegawai,
        @pKodeCabang = @KodeCabang,
        @pKodeJabatan = @KodeJabatan;
END
GO
