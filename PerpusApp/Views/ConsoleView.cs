using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerpusApp.Controllers;
using PerpusApp.Models;

namespace PerpusApp.Views
{
    // View untuk menampilkan output ke konsol
    public class ConsoleView
    {
        private readonly PerpustakaanController controller;

        public ConsoleView(PerpustakaanController controller)
        {
            this.controller = controller;
        }

        public async Task TampilkanMenuUtama()
        {
            Console.WriteLine("=== SELAMAT DATANG DI APLIKASI PERPUSTAKAAN ===");
            
            // Tampilkan informasi koneksi database
            Console.WriteLine("Menghubungkan ke database...");
            bool terhubung = await controller.TestKoneksiDatabase();
            if (terhubung)
            {
                Console.WriteLine("Berhasil terhubung ke database!");
            }
            else
            {
                Console.WriteLine("Gagal terhubung ke database! Aplikasi akan tetap berjalan dengan fitur terbatas.");
            }
            
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Tampilkan semua buku");
                Console.WriteLine("2. Cari buku");
                Console.WriteLine("3. Tambah buku");
                Console.WriteLine("4. Pinjam buku");
                Console.WriteLine("5. Kembalikan buku");
                Console.WriteLine("6. Lihat peminjaman aktif");
                Console.WriteLine("7. Lihat daftar pengguna");
                Console.WriteLine("8. Lihat daftar staff");
                Console.WriteLine("9. Buat laporan");
                Console.WriteLine("10. Tampilkan item perpustakaan");
                Console.WriteLine("11. Keluar");
                Console.Write("Pilih menu (1-11): ");

                var pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        await TampilkanSemuaBuku();
                        break;
                    case "2":
                        await CariMenuBuku();
                        break;
                    case "3":
                        await TambahMenuBuku();
                        break;
                    case "4":
                        await PinjamMenuBuku();
                        break;
                    case "5":
                        await KembalikanMenuBuku();
                        break;
                    case "6":
                        await TampilkanPeminjamanAktif();
                        break;
                    case "7":
                        await TampilkanDaftarPengguna();
                        break;
                    case "8":
                        await TampilkanDaftarStaff();
                        break;
                    case "9":
                        await MenuLaporan();
                        break;
                    case "10":
                        await TampilkanItemPerpustakaan();
                        break;
                    case "11":
                        Console.WriteLine("Terima kasih telah menggunakan aplikasi perpustakaan!");
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }
            }
        }

        // Helper untuk menampilkan garis tabel
        private string GetTableLine(int[] columnWidths)
        {
            StringBuilder sb = new StringBuilder("+");
            foreach (var width in columnWidths)
            {
                sb.Append(new string('-', width + 2) + "+");
            }
            return sb.ToString();
        }

        // Helper untuk menampilkan baris tabel
        private string GetTableRow(string[] values, int[] columnWidths)
        {
            StringBuilder sb = new StringBuilder("|");
            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i] ?? "";
                sb.Append(" " + value.PadRight(columnWidths[i]) + " |");
            }
            return sb.ToString();
        }

        public async Task TampilkanSemuaBuku()
        {
            Console.WriteLine("\n=== DAFTAR BUKU ===");
            var daftarBuku = await controller.GetSemuaBuku();
            if (daftarBuku.Count == 0)
            {
                Console.WriteLine("Tidak ada buku dalam perpustakaan.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 30, 20, 15, 10 };
            string[] headers = { "ID", "Judul", "Penulis", "Kategori", "Stok" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var buku in daftarBuku)
            {
                string[] values = {
                    buku.Id.ToString(),
                    buku.Judul,
                    buku.Penulis,
                    buku.Kategori,
                    buku.JumlahStok.ToString()
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));
        }

        private async Task CariMenuBuku()
        {
            Console.Write("Masukkan kata kunci: ");
            var kata = Console.ReadLine();
            if (string.IsNullOrEmpty(kata)) return;

            Console.WriteLine($"\n=== HASIL PENCARIAN: '{kata}' ===");
            var hasil = await controller.CariBuku(kata);

            if (hasil.Count == 0)
            {
                Console.WriteLine("Tidak ada buku yang ditemukan.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 30, 20, 15, 10 };
            string[] headers = { "ID", "Judul", "Penulis", "Kategori", "Stok" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var buku in hasil)
            {
                string[] values = {
                    buku.Id.ToString(),
                    buku.Judul,
                    buku.Penulis,
                    buku.Kategori,
                    buku.JumlahStok.ToString()
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));
        }

        private async Task TambahMenuBuku()
        {
            Console.Write("Judul buku: ");
            var judul = Console.ReadLine();
            Console.Write("Penulis: ");
            var penulis = Console.ReadLine();
            Console.Write("Kategori: ");
            var kategori = Console.ReadLine();
            Console.Write("Jumlah stok: ");
            
            int jumlahStok = 1;
            if (!int.TryParse(Console.ReadLine(), out jumlahStok) || jumlahStok < 1)
            {
                jumlahStok = 1;
                Console.WriteLine("Jumlah stok tidak valid, menggunakan nilai default (1).");
            }
            
            if (!string.IsNullOrEmpty(judul) && !string.IsNullOrEmpty(penulis))
            {
                string result = await controller.TambahBuku(judul, penulis, kategori ?? "Umum", jumlahStok);
                Console.WriteLine(result);
            }
        }

        private async Task PinjamMenuBuku()
        {
            // Tampilkan buku yang tersedia
            Console.WriteLine("\n=== BUKU YANG TERSEDIA ===");
            var daftarBuku = await controller.GetSemuaBuku();
            var bukuTersedia = daftarBuku.Where(b => b.JumlahStok > 0).ToList();
            
            if (bukuTersedia.Count == 0)
            {
                Console.WriteLine("Tidak ada buku yang tersedia untuk dipinjam.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 30, 20, 15, 10 };
            string[] headers = { "ID", "Judul", "Penulis", "Kategori", "Stok" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var buku in bukuTersedia)
            {
                string[] values = {
                    buku.Id.ToString(),
                    buku.Judul,
                    buku.Penulis,
                    buku.Kategori,
                    buku.JumlahStok.ToString()
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));

            Console.Write("\nMasukkan ID buku yang ingin dipinjam: ");
            if (int.TryParse(Console.ReadLine(), out int idPinjam))
            {
                string result = await controller.PinjamBuku(idPinjam);
                Console.WriteLine(result);
            }
        }

        private async Task KembalikanMenuBuku()
        {
            // Tampilkan peminjaman aktif
            Console.WriteLine("\n=== PEMINJAMAN AKTIF ===");
            var peminjamanList = await controller.GetDaftarPeminjamanAktif();
            
            if (peminjamanList.Count == 0)
            {
                Console.WriteLine("Tidak ada peminjaman aktif saat ini.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 30, 25, 15 };
            string[] headers = { "ID", "Judul Buku", "Nama Peminjam", "Tanggal Pinjam" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var peminjaman in peminjamanList)
            {
                string[] values = {
                    peminjaman.Id.ToString(),
                    peminjaman.Buku.Judul,
                    peminjaman.Pengguna.Nama,
                    peminjaman.TanggalPinjam.ToString("dd/MM/yyyy")
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));

            Console.Write("\nMasukkan ID peminjaman yang ingin dikembalikan: ");
            if (int.TryParse(Console.ReadLine(), out int idPeminjaman))
            {
                var peminjaman = peminjamanList.FirstOrDefault(p => p.Id == idPeminjaman);
                if (peminjaman != null)
                {
                    string result = await controller.KembalikanBuku(idPeminjaman, peminjaman.IdBuku);
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("ID peminjaman tidak valid.");
                }
            }
        }

        private async Task TampilkanPeminjamanAktif()
        {
            Console.WriteLine("\n=== DAFTAR PEMINJAMAN AKTIF ===");
            var peminjamanList = await controller.GetDaftarPeminjamanAktif();
            
            if (peminjamanList.Count == 0)
            {
                Console.WriteLine("Tidak ada peminjaman aktif saat ini.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 30, 20, 20, 15 };
            string[] headers = { "ID", "Judul Buku", "Peminjam", "Petugas", "Tanggal Pinjam" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var peminjaman in peminjamanList)
            {
                string[] values = {
                    peminjaman.Id.ToString(),
                    peminjaman.Buku.Judul,
                    peminjaman.Pengguna.Nama,
                    peminjaman.Staff.Nama,
                    peminjaman.TanggalPinjam.ToString("dd/MM/yyyy")
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));
        }

        private async Task TampilkanDaftarPengguna()
        {
            Console.WriteLine("\n=== DAFTAR PENGGUNA ===");
            var penggunaList = await controller.GetDaftarPengguna();
            
            if (penggunaList.Count == 0)
            {
                Console.WriteLine("Tidak ada pengguna terdaftar.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 25, 30, 15 };
            string[] headers = { "ID", "Nama", "Alamat", "No. Telepon" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var pengguna in penggunaList)
            {
                string[] values = {
                    pengguna.Id.ToString(),
                    pengguna.Nama,
                    pengguna.Alamat,
                    pengguna.NoTelepon
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));
        }

        private async Task TampilkanDaftarStaff()
        {
            Console.WriteLine("\n=== DAFTAR STAFF ===");
            var staffList = await controller.GetDaftarStaff();
            
            if (staffList.Count == 0)
            {
                Console.WriteLine("Tidak ada staff terdaftar.");
                return;
            }

            // Tentukan lebar kolom
            int[] columnWidths = { 4, 25, 20, 15 };
            string[] headers = { "ID", "Nama", "Jabatan", "No. Telepon" };

            // Cetak header tabel
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));

            // Cetak baris data
            foreach (var staff in staffList)
            {
                string[] values = {
                    staff.Id.ToString(),
                    staff.Nama,
                    staff.Jabatan,
                    staff.NoTelepon
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            Console.WriteLine(GetTableLine(columnWidths));
            
            // Pilih staff default
            Console.Write("\nMasukkan ID staff untuk digunakan sebagai petugas (kosongkan untuk tetap menggunakan default): ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int staffId))
            {
                var staff = staffList.FirstOrDefault(s => s.Id == staffId);
                if (staff != null)
                {
                    controller.SetDefaultIds(staffId, 1); // Tetap gunakan pengguna default 1
                    Console.WriteLine($"Staff default berhasil diubah ke: {staff.Nama}");
                }
                else
                {
                    Console.WriteLine("ID staff tidak valid.");
                }
            }
        }
        
        // Menu untuk menampilkan laporan (Builder Pattern)
        private async Task MenuLaporan()
        {
            Console.WriteLine("\n=== BUAT LAPORAN ===");
            Console.WriteLine("1. Laporan Bulanan");
            Console.WriteLine("2. Laporan Inventaris");
            Console.WriteLine("3. Laporan Anggota");
            Console.Write("Pilih jenis laporan (1-3): ");
            
            var pilihan = Console.ReadLine();
            
            Console.Write("Masukkan nama penyusun: ");
            string penyusun = Console.ReadLine() ?? "Admin";
            
            LaporanPerpustakaan laporan = null;
            
            switch (pilihan)
            {
                case "1":
                    laporan = await controller.BuatLaporanBulanan(penyusun);
                    break;
                case "2":
                    laporan = await controller.BuatLaporanInventaris(penyusun);
                    break;
                case "3":
                    laporan = await controller.BuatLaporanAnggota(penyusun);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    return;
            }
            
            if (laporan != null)
            {
                Console.WriteLine("\n" + laporan.ToString());
                Console.WriteLine("\nLaporan berhasil dibuat!");
            }
        }
        
        // Menu untuk menampilkan item perpustakaan (Factory Method Pattern)
        private async Task TampilkanItemPerpustakaan()
        {
            Console.WriteLine("\n=== ITEM PERPUSTAKAAN ===");
            Console.WriteLine("Menampilkan semua jenis item perpustakaan menggunakan Factory Method Pattern:");
            
            var daftarBuku = await controller.GetSemuaBuku();
            var items = controller.GetPerpustakaanItems(daftarBuku);
            
            Console.WriteLine($"\nTotal item: {items.Count}\n");
            
            // Tampilkan semua item
            int[] columnWidths = { 4, 50, 15 };
            string[] headers = { "ID", "Informasi", "Status" };
            
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine(GetTableRow(headers, columnWidths));
            Console.WriteLine(GetTableLine(columnWidths));
            
            foreach (var item in items)
            {
                string[] values = {
                    item.Id.ToString(),
                    item.GetDisplayInfo(),
                    item.IsAvailable() ? "Tersedia" : "Tidak Tersedia"
                };
                Console.WriteLine(GetTableRow(values, columnWidths));
            }
            
            Console.WriteLine(GetTableLine(columnWidths));
            Console.WriteLine("\nCatatan: Item di atas dibuat menggunakan Factory Method Pattern.");
            Console.WriteLine("Setiap jenis item (Buku, Majalah, Media Digital) memiliki factory-nya masing-masing.");
        }
    }
}