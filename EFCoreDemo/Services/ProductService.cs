using EFCoreDemo.Models;
using EFCoreDemo.Repositories;

namespace EFCoreDemo.Services;

/// <summary>
/// Tầng business logic (nghiệp vụ).
/// Nhiệm vụ: kiểm tra dữ liệu hợp lệ, quyết định luồng xử lý,
/// rồi gọi ProductRepository để thực sự đọc/ghi database.
///
/// Đây là nơi trả lời câu hỏi "nghiệp vụ cho phép hay không",
/// ví dụ: tên không được rỗng, giá không được âm, số lượng không được âm.
///
/// KHÔNG truy cập AppDbContext trực tiếp (đã có Repository lo).
/// KHÔNG chứa Console.ReadLine()/Console.WriteLine()/menu/UI.
///
/// Nhờ vậy, sau này nếu bạn viết ASP.NET Core Web API, Controller chỉ cần
/// gọi lại đúng class này — toàn bộ quy tắc nghiệp vụ được tái sử dụng.
/// </summary>
public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public List<Product> GetAll()
    {
        return _repository.GetAll();
    }

    public Product? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public List<Product> Search(string keyword)
    {
        return _repository.Search(keyword);
    }

    /// <summary>
    /// Thêm sản phẩm sau khi kiểm tra dữ liệu hợp lệ.
    /// Ném ArgumentException nếu dữ liệu sai để tầng UI hiển thị thông báo.
    /// </summary>
    public Product Add(string name, decimal price, int quantity)
    {
        Validate(name, price, quantity);

        Product product = new Product
        {
            Name = name.Trim(),
            Price = price,
            Quantity = quantity
        };

        return _repository.Add(product);
    }

    /// <summary>
    /// Sửa sản phẩm theo Id.
    /// Trả về false nếu không tìm thấy sản phẩm.
    /// </summary>
    public bool Update(int id, string name, decimal price, int quantity)
    {
        Validate(name, price, quantity);

        Product? product = _repository.GetById(id);

        if (product == null)
        {
            return false;
        }

        product.Name = name.Trim();
        product.Price = price;
        product.Quantity = quantity;

        return _repository.Update(product);
    }

    /// <summary>
    /// Xóa sản phẩm theo Id. Trả về false nếu không tìm thấy.
    /// </summary>
    public bool Delete(int id)
    {
        return _repository.Delete(id);
    }

    /// <summary>
    /// Các quy tắc nghiệp vụ dùng chung cho cả Add và Update.
    ///
    /// Lưu ý dạy học: kiểm tra ở đây là "phòng thủ nhiều lớp".
    /// - ConsoleUI đã chặn chuỗi rỗng và số sai định dạng.
    /// - Service vẫn kiểm tra lại, vì Service có thể bị gọi từ Web API,
    ///   từ unit test, hay từ một UI khác mà không đi qua ConsoleUI.
    /// </summary>
    private static void Validate(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tên sản phẩm không được để trống.");
        }

        if (price < 0)
        {
            throw new ArgumentException("Giá không được âm.");
        }

        if (quantity < 0)
        {
            throw new ArgumentException("Số lượng không được âm.");
        }
    }
}