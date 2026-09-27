using System.Text;
using EFCoreDemo.ConsoleUI;
using EFCoreDemo.Data;
using EFCoreDemo.Repositories;
using EFCoreDemo.Services;

// ==========================================================
// Program.cs: Composition Root
//
// Luồng:
// AppDbContext
//      ↓
// ProductRepository
//      ↓
// ProductService
//      ↓
// ProductConsole
// ==========================================================

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

using AppDbContext context = new AppDbContext();

ProductRepository repository = new ProductRepository(context);
ProductService service = new ProductService(repository);
ProductConsole console = new ProductConsole(service);

console.Run();