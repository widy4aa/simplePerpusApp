using System;

namespace PerpusApp.Models
{
    // Class untuk merepresentasikan Buku
    public class Buku
    {
        public int Id { get; set; }
        public string Judul { get; set; }   
        public string Penulis { get; set; }
        public string Kategori { get; set; }
        public int JumlahStok { get; set; }

        public Buku()
        {
            // Default constructor for database
        }

        public Buku(int id, string judul, string penulis, string kategori, int jumlahStok = 1)
        {
            Id = id;
            Judul = judul;
            Penulis = penulis;
            Kategori = kategori;
            JumlahStok = jumlahStok;
        }

        public override string ToString()
        {
            return $"[{Id}] {Judul} - {Penulis} ({Kategori}) - Stok: {JumlahStok}";
        }
    }
}