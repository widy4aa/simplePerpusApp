using System;
using System.Threading.Tasks;
using PerpusApp.Controllers;
using PerpusApp.Data;
using PerpusApp.Views;

namespace PerpusApp
{
    // Program utama
    class Program
    {
        static async Task Main(string[] args)
        {
            // Connection string untuk PostgreSQL (disembunyikan dari tampilan)
            string connectionString = "Host=interchange.proxy.rlwy.net;Port=13569;Database=railway;Username=postgres;Password=OFpjBpURdQKFJpiYsLDzGqBMtSQQmCvy";
            
            // Inisialisasi Database Service
            var dbService = new DatabaseService(connectionString);
            
            // Inisialisasi Controller
            var controller = new PerpustakaanController(dbService);
            
            // Inisialisasi View
            var view = new ConsoleView(controller);
            
            // Tampilkan UI
            await view.TampilkanMenuUtama();
        }
    }
}
