using Xunit;
using NinjaDB.Services;
using NinjaDB.Models;
using NinjaLedgerDB_Test.TestSupport;
using Microsoft.EntityFrameworkCore;

namespace NinjaLedgerDB_Test.Services
{
    public class CustomerServiceTests : DatabaseTestBase
    {
        [Fact]
        public void Create_AddsCustomer()
        {
            var service = new CustomerService(Context);
            var customer = new Customers { FirstName = "Tyler", LastName = "Test", Email = "tyler.test@example.com" };
            service.Create(customer);
            var result = service.GetById(customer.CustomerId);
            Assert.NotNull(result);
            Assert.Equal("Tyler", result!.FirstName);
        }

        [Fact]
        public void UpdateCustomer_ChangesLastName()
        {
            var service = new CustomerService(Context);
            var customer = new Customers { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };
            service.Create(customer);
            customer.LastName = "Updated";
            service.Update(customer);
            var result = service.GetById(customer.CustomerId);
            Assert.NotNull(result);
            Assert.Equal("Updated", result!.LastName);
        }

        [Fact]
        public void DeleteCustomer_RemovesFromDb()
        {
            var service = new CustomerService(Context);
            var customer = new Customers { FirstName = "Mark", LastName = "Smith", Email = "mark.smith@example.com" };
            service.Create(customer);
            service.Delete(customer.CustomerId);
            using var readCtx = TestDb.GetContext();
            var readService = new CustomerService(readCtx);
            var result = readService.GetById(customer.CustomerId);
            Assert.Null(result);
        }

        [Fact]
        public void Create_DuplicateEmail_Throws_DbUpdateException()
        {
            var service = new CustomerService(Context);
            var c1 = new Customers { FirstName = "A", LastName = "B", Email = "dup@example.com" };
            service.Create(c1);
            using var ctx2 = TestDb.GetContext();
            var service2 = new CustomerService(ctx2);
            var c2 = new Customers { FirstName = "C", LastName = "D", Email = "dup@example.com" };
            Assert.Throws<DbUpdateException>(() => service2.Create(c2));
        }

        [Fact]
        public void DeleteCustomer_WithOrders_Throws_DbUpdateException()
        {
            var customers = new CustomerService(Context);
            var products = new ProductService(Context);
            var orders = new OrderService(Context);
            var customer = new Customers { FirstName = "With", LastName = "Orders", Email = "with.orders@example.com" };
            customers.Create(customer);
            var product = new Products { ProductName = "TestProduct", UnitPrice = 10.00M };
            products.Create(product);
            orders.Create(new Orders { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = 1 });
            using var ctx2 = TestDb.GetContext();
            var customers2 = new CustomerService(ctx2);
            Assert.Throws<DbUpdateException>(() => customers2.Delete(customer.CustomerId));
        }
    }
}

