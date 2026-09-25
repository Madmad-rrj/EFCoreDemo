using Microsoft.EntityFrameworkCore;

Console.WriteLine("===== QUẢN LÝ SẢN PHẨM =====");

using (var context = new AppDbContext())
{
    bool running = true;

    while (running)
    {
        Console.WriteLine("\n===== MENU =====");
        Console.WriteLine("1. Thêm sản phẩm");
        Console.WriteLine("2. Xem danh sách");
        Console.WriteLine("3. Sửa sản phẩm");
        Console.WriteLine("4. Xóa sản phẩm");
        Console.WriteLine("5. Tìm sản phẩm");
        Console.WriteLine("0. Thoát");

        Console.Write("Chọn: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateProduct(context);
                break;

            case "2":
                ReadProducts(context);
                break;

            case "3":
                UpdateProduct(context);
                break;

            case "4":
                DeleteProduct(context);
                break;

            case "5":
                SearchProduct(context);
                break;

            case "0":
                running = false;
                break;

            default:
                Console.WriteLine("Lựa chọn không hợp lệ!");
                break;
        }
    }
}

Console.WriteLine("\n===== CHƯƠNG TRÌNH KẾT THÚC =====");


// ==================================================
// CREATE
// ==================================================

static void CreateProduct(AppDbContext context)
{
    Console.WriteLine("\n===== THÊM SẢN PHẨM =====");

    Console.Write("Tên sản phẩm: ");
    string name = Console.ReadLine();

    Console.Write("Giá: ");
    decimal price = decimal.Parse(Console.ReadLine());

    Console.Write("Số lượng: ");
    int quantity = int.Parse(Console.ReadLine());

    Product product = new Product
    {
        Name = name,
        Price = price,
        Quantity = quantity
    };

    context.Products.Add(product);
    context.SaveChanges();

    Console.WriteLine("Đã thêm sản phẩm!");
}


// ==================================================
// READ
// ==================================================

static void ReadProducts(AppDbContext context)
{
    Console.WriteLine("\n===== DANH SÁCH SẢN PHẨM =====");

    var products = context.Products.ToList();

    if (products.Count == 0)
    {
        Console.WriteLine("Chưa có sản phẩm.");
        return;
    }

    foreach (var product in products)
    {
        Console.WriteLine(
            $"ID: {product.Id} | " +
            $"Tên: {product.Name} | " +
            $"Giá: {product.Price} | " +
            $"Số lượng: {product.Quantity}"
        );
    }
}


// ==================================================
// UPDATE
// ==================================================

static void UpdateProduct(AppDbContext context)
{
    Console.WriteLine("\n===== SỬA SẢN PHẨM =====");

    Console.Write("Nhập ID sản phẩm cần sửa: ");
    int id = int.Parse(Console.ReadLine());

    var product = context.Products
        .FirstOrDefault(p => p.Id == id);

    if (product == null)
    {
        Console.WriteLine("Không tìm thấy sản phẩm!");
        return;
    }

    Console.Write("Tên mới: ");
    product.Name = Console.ReadLine();

    Console.Write("Giá mới: ");
    product.Price = decimal.Parse(Console.ReadLine());

    Console.Write("Số lượng mới: ");
    product.Quantity = int.Parse(Console.ReadLine());

    context.SaveChanges();

    Console.WriteLine("Đã cập nhật sản phẩm!");
}


// ==================================================
// DELETE
// ==================================================

static void DeleteProduct(AppDbContext context)
{
    Console.WriteLine("\n===== XÓA SẢN PHẨM =====");

    Console.Write("Nhập ID sản phẩm cần xóa: ");
    int id = int.Parse(Console.ReadLine());

    var product = context.Products
        .FirstOrDefault(p => p.Id == id);

    if (product == null)
    {
        Console.WriteLine("Không tìm thấy sản phẩm!");
        return;
    }

    context.Products.Remove(product);
    context.SaveChanges();

    Console.WriteLine("Đã xóa sản phẩm!");
}


// ==================================================
// SEARCH
// ==================================================

static void SearchProduct(AppDbContext context)
{
    Console.WriteLine("\n===== TÌM SẢN PHẨM =====");

    Console.Write("Nhập tên cần tìm: ");
    string keyword = Console.ReadLine();

    var products = context.Products
        .Where(p => p.Name.Contains(keyword))
        .ToList();

    if (products.Count == 0)
    {
        Console.WriteLine("Không tìm thấy sản phẩm.");
        return;
    }

    foreach (var product in products)
    {
        Console.WriteLine(
            $"ID: {product.Id} | " +
            $"Tên: {product.Name} | " +
            $"Giá: {product.Price} | " +
            $"Số lượng: {product.Quantity}"
        );
    }
}