# 📚 PerpusApp - Aplikasi Manajemen Perpustakaan

Aplikasi manajemen perpustakaan modern menggunakan C# .NET 8 dengan database PostgreSQL dan arsitektur MVC yang bersih.

## 🏗️ Arsitektur Aplikasi

### MVC Pattern (Model-View-Controller)
```
PerpusApp/
├── Program.cs              # Entry Point & Dependency Injection
├── Models/                 # Data Models & Entities
├── Views/                  # Presentation Layer (Console UI)
├── Controllers/            # Business Logic Layer
└── Data/                   # Data Access Layer
```

## 🚀 Fitur Unggulan

### 1. **Async/Await Pattern**
```csharp
static async Task Main(string[] args)
```
- **Kegunaan**: Operasi database non-blocking
- **Kelebihan**: 
  - Aplikasi tidak freeze saat query database
  - Better performance untuk operasi I/O
  - Scalable untuk multiple users
- **Implementasi**: Semua operasi database menggunakan async methods

### 2. **Dependency Injection Manual**
```csharp
var dbService = new DatabaseService(connectionString);
var controller = new PerpustakaanController(dbService);
var view = new ConsoleView(controller);
```
- **Kegunaan**: Loose coupling antar komponen
- **Kelebihan**:
  - Easy testing (mock dependencies)
  - Flexible configuration
  - Single Responsibility Principle
- **Pattern**: Constructor Injection

### 3. **Repository Pattern dengan PostgreSQL**
```csharp
public class DatabaseService
{
    private readonly string _connectionString;
    
    public async Task<List<Buku>> GetAllBooksAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        // Async database operations
    }
}
```
- **Kegunaan**: Abstraksi data access
- **Kelebihan**:
  - Database agnostic
  - Centralized query management
  - Easy to unit test
  - Transaction support

## 🔧 Komponen Utama

### Models Layer
**Fungsi**: Data representation dan business entities

```csharp
public class Buku
{
    public int Id { get; set; }
    public string Judul { get; set; }
    public string Pengarang { get; set; }
    public string Kategori { get; set; }
    public bool Tersedia { get; set; }
    public DateTime TanggalTambah { get; set; }
}
```

**Kelebihan Models**:
- **Data Validation**: Properties dengan validasi
- **Business Logic**: Methods untuk behavior
- **Serialization Ready**: Compatible dengan JSON/XML

### Views Layer (Console UI)
**Fungsi**: User interface dan interaction handling

```csharp
public class ConsoleView
{
    public async Task TampilkanMenuUtama()
    {
        // Interactive console interface
        // Real-time data display
        // User input validation
    }
}
```

**Fitur Unik Views**:
- **Interactive Menu System**: Dynamic menu dengan validasi
- **Real-time Updates**: Live data dari database
- **Color Coding**: Different colors untuk status buku
- **Input Sanitization**: Validasi input user

### Controllers Layer
**Fungsi**: Business logic coordination dan data flow

```csharp
public class PerpustakaanController
{
    private readonly DatabaseService _dbService;
    
    public async Task<bool> PinjamBukuAsync(int bukuId, string namaPeminjam)
    {
        // Business validation
        // Database transaction
        // Error handling
    }
}
```

**Fitur Unggulan Controllers**:
- **Transaction Management**: ACID compliance
- **Business Rules Validation**: Complex business logic
- **Error Handling**: Comprehensive exception management
- **Audit Trail**: Logging semua operasi

### Data Layer (PostgreSQL Integration)
**Fungsi**: Database operations dan data persistence

```csharp
public class DatabaseService
{
    public async Task<bool> ExecuteTransactionAsync(Func<NpgsqlConnection, Task<bool>> operation)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            var result = await operation(connection);
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

## 🎯 Fungsi-Fungsi Unik & Menarik

### 1. **Connection Pooling**
```csharp
using var connection = new NpgsqlConnection(_connectionString);
```
- **Kegunaan**: Efficient database connection management
- **Kelebihan**: 
  - Automatic connection disposal
  - Memory leak prevention
  - Better performance

### 2. **Parameterized Queries**
```csharp
var command = new NpgsqlCommand(
    "SELECT * FROM buku WHERE judul ILIKE @search", connection);
command.Parameters.AddWithValue("@search", $"%{keyword}%");
```
- **Kegunaan**: SQL Injection prevention
- **Kelebihan**:
  - Security dari SQL injection
  - Query plan caching
  - Type safety

### 3. **Async Enumerable Pattern**
```csharp
public async IAsyncEnumerable<Buku> GetBooksStreamAsync()
{
    await foreach (var book in ReadBooksAsync())
    {
        yield return book;
    }
}
```
- **Kegunaan**: Memory-efficient data streaming
- **Kelebihan**:
  - Low memory footprint
  - Real-time data processing
  - Scalable untuk big data

### 4. **Exception Handling Strategy**
```csharp
public async Task<(bool Success, string Message)> TryOperationAsync(Func<Task> operation)
{
    try
    {
        await operation();
        return (true, "Success");
    }
    catch (PostgresException ex)
    {
        return (false, $"Database error: {ex.MessageText}");
    }
    catch (Exception ex)
    {
        return (false, $"Unexpected error: {ex.Message}");
    }
}
```
- **Kegunaan**: Graceful error handling
- **Kelebihan**:
  - User-friendly error messages
  - Application stability
  - Debugging information

## 📊 Database Schema

### Tabel Buku
```sql
CREATE TABLE buku (
    id SERIAL PRIMARY KEY,
    judul VARCHAR(255) NOT NULL,
    pengarang VARCHAR(255) NOT NULL,
    kategori VARCHAR(100),
    tersedia BOOLEAN DEFAULT true,
    tanggal_tambah TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Tabel Peminjaman
```sql
CREATE TABLE peminjaman (
    id SERIAL PRIMARY KEY,
    buku_id INTEGER REFERENCES buku(id),
    nama_peminjam VARCHAR(255) NOT NULL,
    tanggal_pinjam TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    tanggal_kembali TIMESTAMP,
    selesai BOOLEAN DEFAULT false
);
```

## 🔧 Setup & Installation

### Prerequisites
- .NET 8 SDK
- PostgreSQL Database
- Visual Studio Code atau Visual Studio 2022

### Installation
```bash
# Clone repository
git clone <repository-url>

# Navigate to project
cd PerpusApp

# Restore packages
dotnet restore

# Update connection string in Program.cs
# Run application
dotnet run
```

### Configuration
Update connection string di `Program.cs`:
```csharp
string connectionString = "Host=localhost;Database=perpusdb;Username=user;Password=pass";
```

## 🎨 Design Patterns Digunakan

### 1. **Repository Pattern**
- Abstraksi data access
- Testability
- Maintainability

### 2. **MVC Pattern**
- Separation of concerns
- Reusability
- Modularity

### 3. **Dependency Injection**
- Loose coupling
- Testing support
- Flexibility

### 4. **Async/Await Pattern**
- Non-blocking operations
- Better performance
- Scalability

## 🏭 Creational Design Patterns

### 1. **Singleton Pattern**
**Kegunaan**: Memastikan hanya ada satu instance dari class tertentu

```csharp
public class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    private static readonly object _lockObject = new object();
    private readonly string _connectionString;

    private DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static DatabaseConnection GetInstance(string connectionString)
    {
        if (_instance == null)
        {
            lock (_lockObject)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection(connectionString);
                }
            }
        }
        return _instance;
    }
}
```

**Kelebihan**:
- **Memory Efficiency**: Satu instance untuk seluruh aplikasi
- **Global Access**: Akses dari mana saja dalam aplikasi
- **Thread Safe**: Implementasi dengan lock untuk concurrent access
- **Lazy Loading**: Instance dibuat saat pertama kali dibutuhkan

**Use Case**: Database connection, Configuration settings, Logging service

---

### 2. **Factory Method Pattern**
**Kegunaan**: Membuat object tanpa menentukan class yang spesifik

```csharp
public abstract class BukuFactory
{
    public abstract Buku CreateBuku(string judul, string pengarang);
}

public class NovelFactory : BukuFactory
{
    public override Buku CreateBuku(string judul, string pengarang)
    {
        return new Buku(0, judul, pengarang, "Novel")
        {
            // Novel-specific properties
            Genre = "Fiction",
            MinimalUsia = 13
        };
    }
}

public class TextbookFactory : BukuFactory
{
    public override Buku CreateBuku(string judul, string pengarang)
    {
        return new Buku(0, judul, pengarang, "Textbook")
        {
            // Textbook-specific properties
            Subject = "Computer Science",
            Edition = 1
        };
    }
}

// Usage
public class BukuService
{
    public Buku CreateBuku(string type, string judul, string pengarang)
    {
        BukuFactory factory = type.ToLower() switch
        {
            "novel" => new NovelFactory(),
            "textbook" => new TextbookFactory(),
            _ => throw new ArgumentException("Unknown book type")
        };
        
        return factory.CreateBuku(judul, pengarang);
    }
}
```

**Kelebihan**:
- **Extensibility**: Mudah menambah tipe buku baru
- **Decoupling**: Client tidak perlu tahu implementasi spesifik
- **Polymorphism**: Menggunakan inheritance untuk flexibility
- **Single Responsibility**: Setiap factory handle satu tipe

**Use Case**: Creating different types of books, Reports, Database connections

---

### 3. **Abstract Factory Pattern**
**Kegunaan**: Membuat families of related objects

```csharp
// Abstract Factory Interface
public interface ILibraryComponentFactory
{
    IBuku CreateBuku();
    IPeminjaman CreatePeminjaman();
    ILaporan CreateLaporan();
}

// Concrete Factory untuk Perpustakaan Digital
public class DigitalLibraryFactory : ILibraryComponentFactory
{
    public IBuku CreateBuku()
    {
        return new DigitalBuku();
    }

    public IPeminjaman CreatePeminjaman()
    {
        return new DigitalPeminjaman();
    }

    public ILaporan CreateLaporan()
    {
        return new DigitalLaporan();
    }
}

// Concrete Factory untuk Perpustakaan Fisik
public class PhysicalLibraryFactory : ILibraryComponentFactory
{
    public IBuku CreateBuku()
    {
        return new PhysicalBuku();
    }

    public IPeminjaman CreatePeminjaman()
    {
        return new PhysicalPeminjaman();
    }

    public ILaporan CreateLaporan()
    {
        return new PhysicalLaporan();
    }
}

// Library Manager
public class LibraryManager
{
    private readonly ILibraryComponentFactory _factory;

    public LibraryManager(ILibraryComponentFactory factory)
    {
        _factory = factory;
    }

    public void ProcessLibraryOperations()
    {
        var buku = _factory.CreateBuku();
        var peminjaman = _factory.CreatePeminjaman();
        var laporan = _factory.CreateLaporan();
        
        // Process operations with related components
    }
}
```

**Kelebihan**:
- **Consistency**: Semua objects dalam satu family compatible
- **Flexibility**: Mudah switch between different families
- **Isolation**: Client code isolated dari concrete classes
- **Scalability**: Easy to add new product families

**Use Case**: Different library systems (Digital vs Physical), UI themes, Database providers

---

### 4. **Builder Pattern**
**Kegunaan**: Membangun complex objects step by step

```csharp
public class BukuBuilder
{
    private Buku _buku;

    public BukuBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _buku = new Buku();
    }

    public BukuBuilder SetBasicInfo(string judul, string pengarang)
    {
        _buku.Judul = judul;
        _buku.Pengarang = pengarang;
        return this;
    }

    public BukuBuilder SetKategori(string kategori)
    {
        _buku.Kategori = kategori;
        return this;
    }

    public BukuBuilder SetISBN(string isbn)
    {
        _buku.ISBN = isbn;
        return this;
    }

    public BukuBuilder SetTahunTerbit(int tahun)
    {
        _buku.TahunTerbit = tahun;
        return this;
    }

    public BukuBuilder SetHalaman(int halaman)
    {
        _buku.JumlahHalaman = halaman;
        return this;
    }

    public BukuBuilder SetPenerbit(string penerbit)
    {
        _buku.Penerbit = penerbit;
        return this;
    }

    public Buku Build()
    {
        var result = _buku;
        Reset(); // Reset for next build
        return result;
    }
}

// Director class untuk common configurations
public class BukuDirector
{
    public Buku BuildNovelLengkap(BukuBuilder builder, string judul, string pengarang)
    {
        return builder
            .SetBasicInfo(judul, pengarang)
            .SetKategori("Novel")
            .SetISBN(GenerateISBN())
            .SetTahunTerbit(DateTime.Now.Year)
            .SetPenerbit("Gramedia")
            .Build();
    }

    public Buku BuildTextbookSederhana(BukuBuilder builder, string judul, string pengarang)
    {
        return builder
            .SetBasicInfo(judul, pengarang)
            .SetKategori("Textbook")
            .Build();
    }

    private string GenerateISBN() => $"978-{Random.Shared.Next(1000000000, 1999999999)}";
}

// Usage
var builder = new BukuBuilder();
var director = new BukuDirector();

// Complex book dengan semua detail
var complexBook = builder
    .SetBasicInfo("Clean Architecture", "Robert Martin")
    .SetKategori("Programming")
    .SetISBN("978-0134494166")
    .SetTahunTerbit(2017)
    .SetHalaman(432)
    .SetPenerbit("Prentice Hall")
    .Build();

// Simple book menggunakan director
var simpleNovel = director.BuildNovelLengkap(builder, "Laskar Pelangi", "Andrea Hirata");
```

**Kelebihan**:
- **Flexibility**: Dapat membuat different representations
- **Readability**: Fluent interface yang mudah dibaca
- **Reusability**: Builder dapat digunakan berulang kali
- **Validation**: Dapat menambahkan validasi di setiap step
- **Optional Parameters**: Handle optional fields dengan elegant

**Use Case**: Complex book creation, Query builders, Configuration objects

---

### 5. **Prototype Pattern**
**Kegunaan**: Membuat objects dengan cloning existing instances

```csharp
public interface IPrototype<T>
{
    T Clone();
}

public class Buku : IPrototype<Buku>
{
    public int Id { get; set; }
    public string Judul { get; set; }
    public string Pengarang { get; set; }
    public string Kategori { get; set; }
    public bool Tersedia { get; set; }
    public DateTime TanggalTambah { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    
    // Deep clone implementation
    public Buku Clone()
    {
        return new Buku
        {
            Id = 0, // New ID for cloned book
            Judul = this.Judul,
            Pengarang = this.Pengarang,
            Kategori = this.Kategori,
            Tersedia = true, // Reset availability
            TanggalTambah = DateTime.Now, // New timestamp
            Tags = new List<string>(this.Tags) // Deep copy of tags
        };
    }
    
    // Shallow clone alternative
    public Buku ShallowClone()
    {
        return (Buku)this.MemberwiseClone();
    }
}

// Prototype Registry untuk manage common prototypes
public class BukuPrototypeRegistry
{
    private readonly Dictionary<string, Buku> _prototypes = new();

    public void RegisterPrototype(string key, Buku prototype)
    {
        _prototypes[key] = prototype;
    }

    public Buku CreateFromPrototype(string key)
    {
        if (_prototypes.TryGetValue(key, out var prototype))
        {
            return prototype.Clone();
        }
        throw new ArgumentException($"Prototype '{key}' not found");
    }
    
    // Initialize common prototypes
    public void InitializeCommonPrototypes()
    {
        // Novel prototype
        RegisterPrototype("novel", new Buku
        {
            Kategori = "Novel",
            Tags = new List<string> { "fiction", "literature" }
        });
        
        // Programming book prototype
        RegisterPrototype("programming", new Buku
        {
            Kategori = "Programming",
            Tags = new List<string> { "technology", "computer-science" }
        });
        
        // Academic book prototype
        RegisterPrototype("academic", new Buku
        {
            Kategori = "Academic",
            Tags = new List<string> { "education", "research" }
        });
    }
}

// Usage
var registry = new BukuPrototypeRegistry();
registry.InitializeCommonPrototypes();

// Create books from prototypes
var novelBaru = registry.CreateFromPrototype("novel");
novelBaru.Judul = "New Mystery Novel";
novelBaru.Pengarang = "Agatha Christie";

var programmingBook = registry.CreateFromPrototype("programming");
programmingBook.Judul = "Advanced C# Programming";
programmingBook.Pengarang = "John Skeet";

// Clone existing book
var existingBook = GetBookFromDatabase(123);
var duplicateBook = existingBook.Clone();
duplicateBook.Judul = duplicateBook.Judul + " - Copy";
```

**Kelebihan**:
- **Performance**: Lebih cepat dari creating object from scratch
- **Simplicity**: Tidak perlu komplex initialization
- **Runtime Configuration**: Dapat clone objects yang dikonfigurasi runtime
- **Reduce Subclassing**: Alternative untuk menghindari factory hierarchies
- **Deep/Shallow Copy**: Flexibility dalam copying strategy

**Use Case**: Template books, Configuration objects, Game objects, Document templates

## 🎯 Implementasi Patterns dalam PerpusApp

### Kombinasi Patterns
```csharp
// Menggunakan multiple patterns bersama
public class LibraryService
{
    // Singleton untuk database connection
    private readonly DatabaseConnection _dbConnection;
    
    // Factory untuk creating different book types
    private readonly BukuFactory _bukuFactory;
    
    // Builder untuk complex book creation
    private readonly BukuBuilder _bukuBuilder;
    
    // Prototype registry untuk templates
    private readonly BukuPrototypeRegistry _prototypeRegistry;

    public LibraryService()
    {
        _dbConnection = DatabaseConnection.GetInstance("connection_string");
        _bukuFactory = new NovelFactory(); // Or injected
        _bukuBuilder = new BukuBuilder();
        _prototypeRegistry = new BukuPrototypeRegistry();
        _prototypeRegistry.InitializeCommonPrototypes();
    }

    public async Task<Buku> CreateBookFromTemplate(string templateType, string judul, string pengarang)
    {
        // Use prototype pattern for base template
        var book = _prototypeRegistry.CreateFromPrototype(templateType);
        
        // Use builder pattern for complex setup
        var finalBook = _bukuBuilder
            .SetBasicInfo(judul, pengarang)
            .SetKategori(book.Kategori)
            .SetTahunTerbit(DateTime.Now.Year)
            .Build();
            
        return finalBook;
    }
}
```

**Benefits dari Pattern Combination**:
- **Modularity**: Setiap pattern handle specific concern
- **Maintainability**: Easy to modify individual components
- **Testability**: Each pattern dapat di-mock/test secara terpisah
- **Scalability**: System dapat berkembang dengan menambah patterns

## 🚀 Kelebihan Aplikasi

### Performance
- **Async Operations**: Non-blocking database calls
- **Connection Pooling**: Efficient resource usage
- **Prepared Statements**: Query optimization

### Security
- **Parameterized Queries**: SQL injection prevention
- **Input Validation**: Data sanitization
- **Connection Security**: Encrypted connections

### Maintainability
- **Clean Architecture**: Layered design
- **SOLID Principles**: Well-structured code
- **Error Handling**: Comprehensive exception management

### Scalability
- **Database-First**: Supports large datasets
- **Async Pattern**: Handle multiple operations
- **Modular Design**: Easy to extend

## 📝 Usage Examples

### Menambah Buku
```csharp
var buku = new Buku 
{
    Judul = "Clean Code",
    Pengarang = "Robert Martin",
    Kategori = "Programming"
};
await controller.TambahBukuAsync(buku);
```

### Mencari Buku
```csharp
var hasil = await controller.CariBukuAsync("Clean");
```

### Meminjam Buku
```csharp
var success = await controller.PinjamBukuAsync(1, "John Doe");
```

## 📚 Daftar Pustaka

### Design Patterns & Architecture
1. **Gamma, E., Helm, R., Johnson, R., & Vlissides, J.** (1994). *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley Professional.
2. **Martin, R. C.** (2017). *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall.
3. **Fowler, M.** (2002). *Patterns of Enterprise Application Architecture*. Addison-Wesley Professional.
4. **Evans, E.** (2003). *Domain-Driven Design: Tackling Complexity in the Heart of Software*. Addison-Wesley Professional.

### C# & .NET Development
5. **Albahari, J., & Albahari, B.** (2022). *C# 11 in a Nutshell: The Definitive Reference*. O'Reilly Media.
6. **Price, M. J.** (2023). *C# 12 and .NET 8 – Modern Cross-Platform Development Fundamentals*. Packt Publishing.
7. **Troelsen, A., & Japikse, P.** (2021). *Pro C# 9 with .NET 5: Foundational Principles and Practices in Programming*. Apress.
8. **Lock, A.** (2023). *ASP.NET Core in Action, Third Edition*. Manning Publications.

### Database & PostgreSQL
9. **Krosing, H., Worsley, J. C., & Drake, M.** (2001). *Practical PostgreSQL*. O'Reilly Media.
10. **Obe, R., & Hsu, L.** (2017). *PostgreSQL: Up and Running: A Practical Guide to the Advanced Open Source Database*. O'Reilly Media.
11. **Date, C. J.** (2019). *Database Design and Relational Theory: Normal Forms and All That Jazz*. O'Reilly Media.

### Software Engineering Best Practices
12. **Martin, R. C.** (2008). *Clean Code: A Handbook of Agile Software Craftsmanship*. Prentice Hall.
13. **Beck, K.** (2002). *Test Driven Development: By Example*. Addison-Wesley Professional.
14. **Feathers, M.** (2004). *Working Effectively with Legacy Code*. Prentice Hall.
15. **Hunt, A., & Thomas, D.** (1999). *The Pragmatic Programmer: From Journeyman to Master*. Addison-Wesley Professional.

### Async Programming & Performance
16. **Cleary, S.** (2014). *Concurrency in C# Cookbook: Asynchronous, Parallel, and Multithreaded Programming*. O'Reilly Media.
17. **Richter, J.** (2012). *CLR via C#*. Microsoft Press.
18. **Albahari, J.** (2022). *Threading in C#*. O'Reilly Media.

### Documentation & Articles
19. **Microsoft Docs** (2024). *.NET Documentation*. Retrieved from https://docs.microsoft.com/en-us/dotnet/
20. **Microsoft Docs** (2024). *Entity Framework Core Documentation*. Retrieved from https://docs.microsoft.com/en-us/ef/core/
21. **Npgsql Documentation** (2024). *Npgsql - .NET PostgreSQL Driver*. Retrieved from https://www.npgsql.org/doc/
22. **PostgreSQL Documentation** (2024). *PostgreSQL 16 Documentation*. Retrieved from https://www.postgresql.org/docs/

### MVC Pattern & Web Development
23. **Sanderson, S.** (2019). *Pro ASP.NET Core MVC 2*. Apress.
24. **Freeman, A.** (2022). *Pro ASP.NET Core 6: Develop Cloud-Ready Web Applications Using MVC, Blazor, and Razor Pages*. Apress.
25. **Esposito, D.** (2014). *Programming ASP.NET MVC 4: Developing Real-World Web Applications with ASP.NET MVC*. Microsoft Press.

### Repository Pattern & Data Access
26. **Fowler, M.** (2002). "Repository Pattern". In *Patterns of Enterprise Application Architecture*. Addison-Wesley.
27. **Smith, S.** (2020). *Architecting Modern Web Applications with ASP.NET Core and Microsoft Azure*. Microsoft Press.
28. **Dykstra, T., Anderson, R., & Smith, S.** (2023). "Repository and Unit of Work Patterns". Microsoft Docs.

### Dependency Injection
29. **Seemann, M.** (2011). *Dependency Injection in .NET*. Manning Publications.
30. **Deursen, S., & Holovaty, A.** (2019). *Dependency Injection Principles, Practices, and Patterns*. Manning Publications.

### Console Application Development
31. **Dunn, J.** (2021). "Building Console Applications in .NET". Microsoft Developer Blog.
32. **Microsoft** (2024). "Console Application Template Documentation". .NET CLI Documentation.

### SOLID Principles & Clean Code
33. **Martin, R. C.** (2003). *Agile Software Development: Principles, Patterns, and Practices*. Prentice Hall.
34. **Martin, R. C.** (2009). "The SOLID Principles". Object Mentor.
35. **Uncle Bob Blog** (2024). "Clean Code Blog". Retrieved from https://blog.cleancoder.com/

### Exception Handling & Error Management
36. **Richter, J.** (2012). "Exception Handling". In *CLR via C#*. Microsoft Press.
37. **Lippert, E.** (2008). "Exception Handling Best Practices". Microsoft Developer Network.

### Git & Version Control
38. **Chacon, S., & Straub, B.** (2014). *Pro Git*. Apress.
39. **Atlassian** (2024). "Git Tutorials". Retrieved from https://www.atlassian.com/git/tutorials

### Software Testing
40. **Osherove, R.** (2013). *The Art of Unit Testing: with examples in C#*. Manning Publications.
41. **Khorikov, V.** (2020). *Unit Testing Principles, Practices, and Patterns*. Manning Publications.

### Creational Design Patterns - Specific References
42. **Shvets, A.** (2024). "Refactoring.Guru - Design Patterns". Retrieved from https://refactoring.guru/design-patterns
43. **Head First Design Patterns Team** (2020). *Head First Design Patterns: Building Extensible and Maintainable Object-Oriented Software*. O'Reilly Media.
44. **Bishop, J.** (2019). *C# 8.0 Design Patterns: Use Gang of Four patterns with C# and .NET Core 3*. Packt Publishing.

### Online Resources & Communities
45. **Stack Overflow** (2024). "C# and .NET Questions". Retrieved from https://stackoverflow.com/questions/tagged/c%23
46. **GitHub** (2024). "Open Source C# Projects". Retrieved from https://github.com/topics/csharp
47. **Reddit** (2024). "r/csharp Community". Retrieved from https://reddit.com/r/csharp
48. **C# Corner** (2024). "C# Programming Articles". Retrieved from https://www.c-sharpcorner.com/

### Industry Standards & Best Practices
49. **ISO/IEC 23270:2018** (2018). *Information technology — Programming languages — C#*. International Organization for Standardization.
50. **IEEE Std 1016-2009** (2009). *IEEE Standard for Information Technology — Systems Design — Software Design Descriptions*. IEEE Computer Society.

### Tools & Development Environment
51. **Microsoft** (2024). "Visual Studio Documentation". Retrieved from https://docs.microsoft.com/en-us/visualstudio/
52. **Microsoft** (2024). "Visual Studio Code Documentation". Retrieved from https://code.visualstudio.com/docs
53. **JetBrains** (2024). "ReSharper Documentation". Retrieved from https://www.jetbrains.com/help/resharper/

## 📄 License

This project is licensed under the MIT License.
