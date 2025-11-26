using Xunit;
using NinjaDB.Services;
using NinjaDB.Models;
using NinjaLedgerDB_Test.TestSupport;
using Microsoft.EntityFrameworkCore;

namespace NinjaLedgerDB_Test.Services
{
    public class ProductServiceTests : DatabaseTestBase
    {
        [Fact]
        public void Create_AddsProduct()
        {
            var service = new ProductService(Context);

            var product = new Products { ProductName = "Laptop", UnitPrice = 999.99M };
            service.Create(product);

            var result = service.GetById(product.ProductId);
            Assert.NotNull(result);
            Assert.Equal("Laptop", result!.ProductName);
            Assert.Equal(999.99M, result.UnitPrice);
        }

        [Fact]
        public void UpdateProduct_ChangesPrice()
        {
            var service = new ProductService(Context);

            var product = new Products { ProductName = "Phone", UnitPrice = 500M };
            service.Create(product);

            product.UnitPrice = 450M;
            service.Update(product);

            var result = service.GetById(product.ProductId);
            Assert.NotNull(result); 
            Assert.Equal(450M, result!.UnitPrice);
        }

        [Fact]
        public void DeleteProduct_RemovesFromDb()
        {
            var service = new ProductService(Context);

            var product = new Products { ProductName = "Tablet", UnitPrice = 300M };
            service.Create(product);

            service.Delete(product.ProductId);

            // Verify with a fresh context 
            using var readCtx = TestDb.GetContext();
            var readService = new ProductService(readCtx);
            var result = readService.GetById(product.ProductId);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteProduct_WithOrders_Throws_DbUpdateException()
        {
            var products = new ProductService(Context);
            var customers = new CustomerService(Context);
            var orders = new OrderService(Context);

            var product = new Products { ProductName = "Headphones", UnitPrice = 100M };
            products.Create(product);

            var customer = new Customers { FirstName = "Order", LastName = "Test", Email = "order.test@example.com" };
            customers.Create(customer);

            orders.Create(new Orders { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = 1 });

            
            using var ctx2 = TestDb.GetContext();
            var products2 = new ProductService(ctx2);

            Assert.Throws<DbUpdateException>(() => products2.Delete(product.ProductId));
        }
    }
}
