using System;
using System.Collections.Generic;

namespace PerpusApp.Models
{
    /// <summary>
    /// Builder Pattern: Memisahkan konstruksi objek kompleks dari representasinya
    /// sehingga proses konstruksi yang sama dapat membuat representasi yang berbeda.
    /// Pattern ini berguna untuk membuat objek dengan banyak parameter opsional
    /// atau banyak langkah konfigurasi.
    /// </summary>
    public class LaporanPerpustakaan
    {
        public string Judul { get; set; }
        public DateTime Tanggal { get; set; }
        public string PenggunaId { get; set; }
        public string Penyusun { get; set; }
        public List<Buku> DaftarBuku { get; set; } = new List<Buku>();
        public List<Peminjaman> DaftarPeminjaman { get; set; } = new List<Peminjaman>();
        public List<Pengguna> DaftarPengguna { get; set; } = new List<Pengguna>();
        public string KatagantiFooter { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        
        public override string ToString()
        {
            var result = $"=== {Judul} ===\n";
            result += $"Tanggal: {Tanggal:dd/MM/yyyy}\n";
            result += $"Penyusun: {Penyusun}\n";
            
            if (Header != null)
                result += $"\n{Header}\n";
            
            if (DaftarBuku.Count > 0)
            {
                result += "\nDaftar Buku:\n";
                foreach (var buku in DaftarBuku)
                {
                    result += $"- {buku}\n";
                }
            }
            
            if (DaftarPeminjaman.Count > 0)
            {
                result += "\nDaftar Peminjaman:\n";
                foreach (var peminjaman in DaftarPeminjaman)
                {
                    result += $"- {peminjaman}\n";
                }
            }
            
            if (DaftarPengguna.Count > 0)
            {
                result += "\nDaftar Pengguna:\n";
                foreach (var pengguna in DaftarPengguna)
                {
                    result += $"- {pengguna}\n";
                }
            }
            
            if (Footer != null)
                result += $"\n{Footer}\n";
            
            return result;
        }
    }
    
    /// <summary>
    /// Builder: Interface yang mendefinisikan langkah-langkah untuk membuat LaporanPerpustakaan
    /// </summary>
    public interface ILaporanBuilder
    {
        ILaporanBuilder SetJudul(string judul);
        ILaporanBuilder SetTanggal(DateTime tanggal);
        ILaporanBuilder SetPenyusun(string penyusun);
        ILaporanBuilder AddBuku(Buku buku);
        ILaporanBuilder AddBukuRange(IEnumerable<Buku> daftarBuku);
        ILaporanBuilder AddPeminjaman(Peminjaman peminjaman);
        ILaporanBuilder AddPeminjamanRange(IEnumerable<Peminjaman> daftarPeminjaman);
        ILaporanBuilder AddPengguna(Pengguna pengguna);
        ILaporanBuilder AddPenggunaRange(IEnumerable<Pengguna> daftarPengguna);
        ILaporanBuilder SetHeader(string header);
        ILaporanBuilder SetFooter(string footer);
        LaporanPerpustakaan Build();
    }
    
    /// <summary>
    /// Concrete Builder: Implementasi dari ILaporanBuilder
    /// </summary>
    public class LaporanPerpustakaanBuilder : ILaporanBuilder
    {
        private readonly LaporanPerpustakaan _laporan = new LaporanPerpustakaan();
        
        public ILaporanBuilder SetJudul(string judul)
        {
            _laporan.Judul = judul;
            return this;
        }
        
        public ILaporanBuilder SetTanggal(DateTime tanggal)
        {
            _laporan.Tanggal = tanggal;
            return this;
        }
        
        public ILaporanBuilder SetPenyusun(string penyusun)
        {
            _laporan.Penyusun = penyusun;
            return this;
        }
        
        public ILaporanBuilder AddBuku(Buku buku)
        {
            _laporan.DaftarBuku.Add(buku);
            return this;
        }
        
        public ILaporanBuilder AddBukuRange(IEnumerable<Buku> daftarBuku)
        {
            _laporan.DaftarBuku.AddRange(daftarBuku);
            return this;
        }
        
        public ILaporanBuilder AddPeminjaman(Peminjaman peminjaman)
        {
            _laporan.DaftarPeminjaman.Add(peminjaman);
            return this;
        }
        
        public ILaporanBuilder AddPeminjamanRange(IEnumerable<Peminjaman> daftarPeminjaman)
        {
            _laporan.DaftarPeminjaman.AddRange(daftarPeminjaman);
            return this;
        }
        
        public ILaporanBuilder AddPengguna(Pengguna pengguna)
        {
            _laporan.DaftarPengguna.Add(pengguna);
            return this;
        }
        
        public ILaporanBuilder AddPenggunaRange(IEnumerable<Pengguna> daftarPengguna)
        {
            _laporan.DaftarPengguna.AddRange(daftarPengguna);
            return this;
        }
        
        public ILaporanBuilder SetHeader(string header)
        {
            _laporan.Header = header;
            return this;
        }
        
        public ILaporanBuilder SetFooter(string footer)
        {
            _laporan.Footer = footer;
            return this;
        }
        
        public LaporanPerpustakaan Build()
        {
            return _laporan;
        }
    }
    
    /// <summary>
    /// Director: Kelas yang menggunakan builder untuk membuat objek dalam urutan tertentu
    /// </summary>
    public class LaporanDirector
    {
        public LaporanPerpustakaan BuildLaporanBulanan(ILaporanBuilder builder, List<Buku> daftarBuku, List<Peminjaman> daftarPeminjaman, string penyusun)
        {
            return builder
                .SetJudul("Laporan Bulanan Perpustakaan")
                .SetTanggal(DateTime.Now)
                .SetPenyusun(penyusun)
                .AddBukuRange(daftarBuku)
                .AddPeminjamanRange(daftarPeminjaman)
                .SetHeader("Laporan ini berisi statistik bulanan perpustakaan")
                .SetFooter("Dicetak pada " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                .Build();
        }
        
        public LaporanPerpustakaan BuildLaporanInventaris(ILaporanBuilder builder, List<Buku> daftarBuku, string penyusun)
        {
            return builder
                .SetJudul("Laporan Inventaris Buku")
                .SetTanggal(DateTime.Now)
                .SetPenyusun(penyusun)
                .AddBukuRange(daftarBuku)
                .SetHeader("Daftar lengkap inventaris buku perpustakaan")
                .SetFooter("Inventaris per " + DateTime.Now.ToString("MMMM yyyy"))
                .Build();
        }
        
        public LaporanPerpustakaan BuildLaporanAnggota(ILaporanBuilder builder, List<Pengguna> daftarPengguna, string penyusun)
        {
            return builder
                .SetJudul("Laporan Anggota Perpustakaan")
                .SetTanggal(DateTime.Now)
                .SetPenyusun(penyusun)
                .AddPenggunaRange(daftarPengguna)
                .SetHeader("Daftar seluruh anggota perpustakaan")
                .SetFooter("Data anggota per " + DateTime.Now.ToString("MMMM yyyy"))
                .Build();
        }
    }
}