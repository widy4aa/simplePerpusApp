using System;

namespace PerpusApp.Models
{
    public class Peminjaman
    {
        public int Id { get; set; }
        public int IdPengguna { get; set; }
        public int IdBuku { get; set; }
        public int IdStaff { get; set; }
        public DateTime TanggalPinjam { get; set; }
        public DateTime? TanggalKembali { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public Pengguna Pengguna { get; set; }
        public Buku Buku { get; set; }
        public Staff Staff { get; set; }

        public override string ToString()
        {
            return $"[{Id}] Buku: {Buku?.Judul} - Peminjam: {Pengguna?.Nama} - Status: {Status}";
        }
    }
}