using EFCoreDemo.Models;
using EFCoreDemo.Services;

namespace EFCoreDemo.ConsoleUI;

/// <summary>
/// Tầng giao diện Console.
/// Nhiệm vụ: hiển thị menu, đọc input từ bàn phím, gọi ProductService,
/// và hiển thị kết quả trả về.
///
/// Class này KHÔNG được viết context.Products... hay bất kỳ truy vấn EF Core nào.
/// Nó cũng KHÔNG chứa quy tắc nghiệp vụ, chỉ chuẩn bị dữ liệu cho Service xử lý.
///
/// Nhờ tách riêng như vậy, sau này bạn có thể thay ConsoleUI bằng
/// Controller của ASP.NET Core Web API mà không phải viết lại Service/Repository.
/// </summary>
public class ProductConsole
{
    private readonly ProductService _service;

    public ProductConsole(ProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// Vòng lặp chính của chương trình: hiển thị menu cho tới khi chọn 0.
    /// </summary>
    public void Run()
    {
        Console.WriteLine("===== QUẢN LÝ SẢN PHẨM =====");

        bool running = true;

        while (running)
        {
            PrintMenu();

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateProduct();
                    break;

                case "2":
                    ReadProducts();
                    break;

                case "3":
                    UpdateProduct();
                    break;

                case "4":
                    DeleteProduct();
                    break;

                case "5":
                    SearchProduct();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ!");
                    break;
            }
        }

        Console.WriteLine("\n===== CHƯƠNG TRÌNH KẾT THÚC =====");
    }

    private static void PrintMenu()
    {
        Console.WriteLine("\n===== MENU =====");
        Console.WriteLine("1. Thêm sản phẩm");
        Console.WriteLine("2. Xem danh sách");
        Console.WriteLine("3. Sửa sản phẩm");
        Console.WriteLine("4. Xóa sản phẩm");
        Console.WriteLine("5. Tìm sản phẩm");
        Console.WriteLine("0. Thoát");

        Console.Write("Chọn: ");
    }

    // ==================================================
    // CREATE
    // ==================================================

    private void CreateProduct()
    {
        Console.WriteLine("\n===== THÊM SẢN PHẨM =====");

        string name = ReadRequiredText("Tên sản phẩm: ");
        decimal price = ReadDecimal("Giá: ");
        int quantity = ReadInt("Số lượng: ");

        try
        {
            Product product = _service.Add(name, price, quantity);
            Console.WriteLine($"Đã thêm sản phẩm! (ID = {product.Id})");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
    }

    // ==================================================
    // READ
    // ==================================================

    private void ReadProducts()
    {
        Console.WriteLine("\n===== DANH SÁCH SẢN PHẨM =====");

        List<Product> products = _service.GetAll();

        if (products.Count == 0)
        {
            Console.WriteLine("Chưa có sản phẩm.");
            return;
        }

        PrintProducts(products);
    }

    // ==================================================
    // UPDATE
    // ==================================================

    private void UpdateProduct()
    {
        Console.WriteLine("\n===== SỬA SẢN PHẨM =====");

        int id = ReadInt("Nhập ID sản phẩm cần sửa: ");

        // Kiểm tra tồn tại ở tầng Service trước khi hỏi các thông tin mới.
        if (_service.GetById(id) == null)
        {
            Console.WriteLine("Không tìm thấy sản phẩm!");
            return;
        }

        string name = ReadRequiredText("Tên mới: ");
        decimal price = ReadDecimal("Giá mới: ");
        int quantity = ReadInt("Số lượng mới: ");

        try
        {
            bool updated = _service.Update(id, name, price, quantity);

            Console.WriteLine(updated
                ? "Đã cập nhật sản phẩm!"
                : "Không tìm thấy sản phẩm!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
    }

    // ==================================================
    // DELETE
    // ==================================================

    private void DeleteProduct()
    {
        Console.WriteLine("\n===== XÓA SẢN PHẨM =====");

        int id = ReadInt("Nhập ID sản phẩm cần xóa: ");

        bool deleted = _service.Delete(id);

        Console.WriteLine(deleted
            ? "Đã xóa sản phẩm!"
            : "Không tìm thấy sản phẩm!");
    }

    // ==================================================
    // SEARCH
    // ==================================================

    private void SearchProduct()
    {
        Console.WriteLine("\n===== TÌM SẢN PHẨM =====");

        Console.Write("Nhập tên cần tìm: ");
        string keyword = Console.ReadLine() ?? string.Empty;

        List<Product> products = _service.Search(keyword);

        if (products.Count == 0)
        {
            Console.WriteLine("Không tìm thấy sản phẩm.");
            return;
        }

        PrintProducts(products);
    }

    // ==================================================
    // CÁC HÀM HIỂN THỊ / ĐỌC INPUT DÙNG CHUNG
    // ==================================================

    private static void PrintProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            Console.WriteLine(
                $"ID: {product.Id} | " +
                $"Tên: {product.Name} | " +
                $"Giá: {product.Price} | " +
                $"Số lượng: {product.Quantity}"
            );
        }
    }

    /// <summary>
    /// Đọc chuỗi và yêu cầu không được rỗng.
    /// </summary>
    private static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (input.Length > 0)
            {
                return input;
            }

            Console.WriteLine("Giá trị không được để trống, vui lòng nhập lại.");
        }
    }

    /// <summary>
    /// Đọc số thực (decimal), nhập sai định dạng thì yêu cầu nhập lại.
    /// Dùng TryParse thay vì Parse để chương trình không bị crash.
    /// </summary>
    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (decimal.TryParse(input, out decimal value))
            {
                return value;
            }

            Console.WriteLine("Giá không hợp lệ, vui lòng nhập lại (ví dụ: 20000000).");
        }
    }

    /// <summary>
    /// Đọc số nguyên (int), nhập sai định dạng thì yêu cầu nhập lại.
    /// </summary>
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Số không hợp lệ, vui lòng nhập lại (ví dụ: 5).");
        }
    }
}