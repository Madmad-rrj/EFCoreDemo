using EFCoreDemo.ConsoleUI;
using EFCoreDemo.Data;
using EFCoreDemo.Repositories;
using EFCoreDemo.Services;

// ==========================================================
// Program.cs: chỉ khởi tạo (composition root) và bắt đầu chương trình.
// Không chứa CRUD, không đọc/ghi database, không chứa menu.
//
// Luồng khởi tạo:
//   AppDbContext  ->  ProductRepository  ->  ProductService  ->  ProductConsole
//
// "using" để AppDbContext được Dispose (đóng kết nối) khi thoát chương trình.
// Đây là dạng Dependency Injection bằng tay (manual DI):
// tự tay tạo đối tượng rồi truyền vào constructor của tầng trên.
// Bước này chưa cần DI container của ASP.NET Core.
// ==========================================================

using AppDbContext context = new AppDbContext();

ProductRepository repository = new ProductRepository(context);
ProductService service = new ProductService(repository);
ProductConsole console = new ProductConsole(service);

console.Run();