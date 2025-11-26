using Xunit;
using NinjaDB.Services;
using NinjaDB.Models;
using NinjaLedgerDB_Test.TestSupport;

namespace NinjaLedgerDB_Test.Services
{
    public class OrderServiceTests : DatabaseTestBase
    {
        [Fact]
        public void Create_AddsOrder()
        {
            var customers = new CustomerService(Context);
            var products = new ProductService(Context);
            var orders = new OrderService(Context);

            var customer = new Customers { FirstName = "Order", LastName = "Creator", Email = "order.creator@example.com" };
            customers.Create(customer);

            var product = new Products { ProductName = "Keyboard", UnitPrice = 50M };
            products.Create(product);

            var order = new Orders { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = 2 };
            orders.Create(order);

            var result = orders.GetById(order.OrderId);
            Assert.NotNull(result);
            Assert.Equal(2, result!.Quantity);
        }

        [Fact]
        public void UpdateOrder_ChangesQuantity()
        {
            var customers = new CustomerService(Context);
            var products = new ProductService(Context);
            var orders = new OrderService(Context);

            var customer = new Customers { FirstName = "Update", LastName = "Order", Email = "update.order@example.com" };
            customers.Create(customer);

            var product = new Products { ProductName = "Mouse", UnitPrice = 25M };
            products.Create(product);

            var order = new Orders { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = 1 };
            orders.Create(order);

            order.Quantity = 5;
            orders.Update(order);

            var result = orders.GetById(order.OrderId);
            Assert.Equal(5, result!.Quantity);
        }

        [Fact]
        public void DeleteOrder_RemovesFromDatabase()
        {
            var customers = new CustomerService(Context);
            var products = new ProductService(Context);
            var orders = new OrderService(Context);

            var customer = new Customers { FirstName = "Delete", LastName = "Order", Email = "delete.order@example.com" };
            customers.Create(customer);

            var product = new Products { ProductName = "Monitor", UnitPrice = 200M };
            products.Create(product);

            var order = new Orders { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = 1 };
            orders.Create(order);

            orders.Delete(order.OrderId);

            using var readCtx = TestDb.GetContext();
            var readOrders = new OrderService(readCtx);
            var result = readOrders.GetById(order.OrderId);
            Assert.Null(result);
        }
    }
}
