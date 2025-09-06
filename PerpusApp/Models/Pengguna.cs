using System;

namespace PerpusApp.Models
{
    public class Pengguna
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string NoTelepon { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Nama} - {Alamat} - {NoTelepon}";
        }
    }
}