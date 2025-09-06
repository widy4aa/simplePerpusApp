using System;

namespace PerpusApp.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string NoTelepon { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Nama} - {Jabatan} - {NoTelepon}";
        }
    }
}