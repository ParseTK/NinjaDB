using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NinjaDB.Data;
using NinjaDB.Interfaces;
using NinjaDB.Services;
using NinjaDB.Models;

class Program
{
    static void Main()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("NinjaLedgerDB");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("ERROR: Connection string not found!");
            Console.WriteLine("Please run: dotnet user-secrets set \"ConnectionStrings:NinjaLedgerDB\" \"your-connection-string\"");
            return;
        }

        var services = new ServiceCollection();
        services.AddDbContext<NinjaLedgerDbContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

        var provider = services.BuildServiceProvider();

        // Validate the DbContext can be created
        using (var testScope = provider.CreateScope())
        {
            var testContext = testScope.ServiceProvider.GetRequiredService<NinjaLedgerDbContext>();
            Console.WriteLine("Database connection successful!");
        }

        using var scope = provider.CreateScope();

        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Products");
            Console.WriteLine("3. Orders");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
            switch (Console.ReadLine())
            {
                case "1": RunCustomerMenu(customerService); break;
                case "2": RunProductMenu(productService); break;
                case "3": RunOrderMenu(orderService); break;
                case "0": exit = true; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    // --- CUSTOMER MENU ---
    static void RunCustomerMenu(ICustomerService service)
    {
        RunMenu("Customer",
            () => {
                var fn = ReadString("First Name: ");
                var ln = ReadString("Last Name: ");
                var email = ReadString("Email: ");
                try { service.Create(new Customers { FirstName = fn, LastName = ln, Email = email }); }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            },
            () => { foreach (var c in service.GetAll()) Console.WriteLine($"{c.CustomerId}: {c.FirstName} {c.LastName} - {c.Email}"); },
            () => {
                var id = ReadInt("Enter Customer ID to update: ");
                var c = service.GetById(id);
                if (c != null) { c.LastName = ReadString("New Last Name: "); service.Update(c); }
            },
            () => { var id = ReadInt("Enter Customer ID to delete: "); service.Delete(id); }
        );
    }

    // --- PRODUCT MENU ---
    static void RunProductMenu(IProductService service)
    {
        RunMenu("Product",
            () => {
                var name = ReadString("Product Name: ");
                var price = ReadDecimal("Unit Price: ");
                service.Create(new Products { ProductName = name, UnitPrice = price });
            },
            () => { foreach (var p in service.GetAll()) Console.WriteLine($"{p.ProductId}: {p.ProductName} - ${p.UnitPrice}"); },
            () => {
                var id = ReadInt("Enter Product ID to update: ");
                var p = service.GetById(id);
                if (p != null) { p.UnitPrice = ReadDecimal("New Price: "); service.Update(p); }
            },
            () => { var id = ReadInt("Enter Product ID to delete: "); service.Delete(id); }
        );
    }

    // --- ORDER MENU ---
    static void RunOrderMenu(IOrderService service)
    {
        RunMenu("Order",
            () => {
                var custId = ReadInt("Customer ID: ");
                var prodId = ReadInt("Product ID: ");
                var qty = ReadInt("Quantity: ");
                service.Create(new Orders { CustomerId = custId, ProductId = prodId, Quantity = qty });
            },
            () => { foreach (var o in service.GetAll()) Console.WriteLine($"Order {o.OrderId}: Customer {o.CustomerId} bought Product {o.ProductId} x{o.Quantity}"); },
            () => {
                var id = ReadInt("Enter Order ID to update: ");
                var o = service.GetById(id);
                if (o != null) { o.Quantity = ReadInt("New Quantity: "); service.Update(o); }
            },
            () => { var id = ReadInt("Enter Order ID to delete: "); service.Delete(id); }
        );
    }

    // --- GENERIC MENU RUNNER ---
    static void RunMenu(string entity, Action create, Action read, Action update, Action delete)
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine($"\n--- {entity} Menu ---");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. View All");
            Console.WriteLine("3. Update");
            Console.WriteLine("4. Delete");
            Console.WriteLine("0. Back");
            Console.Write("Choice: ");
            switch (Console.ReadLine())
            {
                case "1": create(); break;
                case "2": read(); break;
                case "3": update(); break;
                case "4": delete(); break;
                case "0": back = true; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    // --- INPUT HELPERS ---
    static int ReadInt(string prompt)
    {
        Console.Write(prompt);
        int.TryParse(Console.ReadLine(), out var value);
        return value;
    }

    static decimal ReadDecimal(string prompt)
    {
        Console.Write(prompt);
        decimal.TryParse(Console.ReadLine(), out var value);
        return value;
    }

    static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }
}