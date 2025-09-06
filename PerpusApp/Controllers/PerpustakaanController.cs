using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerpusApp.Data;
using PerpusApp.Models;

namespace PerpusApp.Controllers
{
    // Controller untuk mengelola perpustakaan
    public class PerpustakaanController
    {
        private readonly DatabaseService _dbService;
        private int _defaultStaffId = 1; // Default staff ID untuk memudahkan pengujian
        private int _defaultPenggunaId = 1; // Default pengguna ID untuk memudahkan pengujian
        
        // Builder untuk laporan
        private readonly LaporanDirector _laporanDirector;
        private readonly ILaporanBuilder _laporanBuilder;

        public PerpustakaanController(DatabaseService dbService)
        {
            _dbService = dbService;
            
            // Inisialisasi Builder pattern
            _laporanBuilder = new LaporanPerpustakaanBuilder();
            _laporanDirector = new LaporanDirector();
        }

        public async Task<bool> TestKoneksiDatabase()
        {
            return await _dbService.TestConnectionAsync();
        }

        public async Task<List<Buku>> GetSemuaBuku()
        {
            return await _dbService.GetSemuaBukuAsync();
        }

        public async Task<List<Buku>> CariBuku(string kata)
        {
            return await _dbService.CariBukuAsync(kata);
        }

        public async Task<string> TambahBuku(string judul, string penulis, string kategori, int jumlahStok = 1)
        {
            bool berhasil = await _dbService.TambahBukuAsync(judul, penulis, kategori, jumlahStok);
            return berhasil 
                ? $"Buku '{judul}' berhasil ditambahkan!" 
                : "Gagal menambahkan buku. Silakan coba lagi.";
        }

        public async Task<string> PinjamBuku(int idBuku)
        {
            bool berhasil = await _dbService.PinjamBukuAsync(idBuku, _defaultPenggunaId, _defaultStaffId);
            if (!berhasil)
            {
                // Cek apakah buku masih tersedia
                var daftarBuku = await _dbService.GetSemuaBukuAsync();
                var buku = daftarBuku.FirstOrDefault(b => b.Id == idBuku);
                
                if (buku == null)
                {
                    return "Buku tidak ditemukan!";
                }
                
                if (buku.JumlahStok <= 0)
                {
                    return "Buku sedang tidak tersedia!";
                }
                
                return "Gagal meminjam buku. Silakan coba lagi.";
            }
            
            return "Buku berhasil dipinjam!";
        }

        public async Task<string> KembalikanBuku(int idPeminjaman, int idBuku)
        {
            bool berhasil = await _dbService.KembalikanBukuAsync(idPeminjaman, idBuku);
            return berhasil 
                ? "Buku berhasil dikembalikan!" 
                : "Gagal mengembalikan buku. Periksa ID peminjaman atau buku sudah dikembalikan.";
        }

        public async Task<List<Peminjaman>> GetDaftarPeminjamanAktif()
        {
            return await _dbService.GetPeminjamanAktifAsync();
        }

        public async Task<List<Pengguna>> GetDaftarPengguna()
        {
            return await _dbService.GetSemuaPenggunaAsync();
        }

        public async Task<List<Staff>> GetDaftarStaff()
        {
            return await _dbService.GetSemuaStaffAsync();
        }

        // Setter untuk default staff dan pengguna ID
        public void SetDefaultIds(int staffId, int penggunaId)
        {
            _defaultStaffId = staffId;
            _defaultPenggunaId = penggunaId;
        }
        
        // Metode untuk membuat laporan menggunakan Builder Pattern
        public async Task<LaporanPerpustakaan> BuatLaporanBulanan(string penyusun)
        {
            var daftarBuku = await GetSemuaBuku();
            var daftarPeminjaman = await GetDaftarPeminjamanAktif();
            
            return _laporanDirector.BuildLaporanBulanan(_laporanBuilder, daftarBuku, daftarPeminjaman, penyusun);
        }
        
        public async Task<LaporanPerpustakaan> BuatLaporanInventaris(string penyusun)
        {
            var daftarBuku = await GetSemuaBuku();
            
            return _laporanDirector.BuildLaporanInventaris(_laporanBuilder, daftarBuku, penyusun);
        }
        
        public async Task<LaporanPerpustakaan> BuatLaporanAnggota(string penyusun)
        {
            var daftarPengguna = await GetDaftarPengguna();
            
            return _laporanDirector.BuildLaporanAnggota(_laporanBuilder, daftarPengguna, penyusun);
        }
        
        // Metode untuk menggunakan Factory Method Pattern
        public List<IPerpustakaanItem> GetPerpustakaanItems(List<Buku> daftarBuku)
        {
            var items = new List<IPerpustakaanItem>();
            
            foreach (var buku in daftarBuku)
            {
                var factory = new BukuItemFactory(buku);
                items.Add(factory.CreateItem());
            }
            
            // Contoh menambahkan majalah dan media digital
            var majalahFactory = new MajalahItemFactory(1001, "National Geographic", "Juni 2023", 3);
            items.Add(majalahFactory.CreateItem());
            
            var mediaDigitalFactory = new MediaDigitalItemFactory(2001, "Kursus Pemrograman C#", "Video", true);
            items.Add(mediaDigitalFactory.CreateItem());
            
            return items;
        }
    }
}