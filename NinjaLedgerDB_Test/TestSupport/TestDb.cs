using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using NinjaDB.Data;

namespace NinjaLedgerDB_Test.TestSupport
{
    public static class TestDb
    {
        // Read connection string from User Secrets or Environment Variables
        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(TestDb).Assembly)
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("NinjaLedgerDB_Test") 
                ?? throw new InvalidOperationException(
                    "Test connection string not found! Run: dotnet user-secrets set \"ConnectionStrings:NinjaLedgerDB_Test\" \"your-test-connection-string\"");
        }

        // Create a fresh DbContext
        public static NinjaLedgerDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<NinjaLedgerDbContext>()
                .UseSqlServer(GetConnectionString())
                .Options;

            var context = new NinjaLedgerDbContext(options);
            return context;
        }

        // Reset database state before each test (order matters due to FKs)
        public static void Reset(NinjaLedgerDbContext context)
        {
            // Clear dependents first to avoid FK conflicts
            context.Database.ExecuteSqlRaw("DELETE FROM Orders;");
            context.Database.ExecuteSqlRaw("DELETE FROM Products;");
            context.Database.ExecuteSqlRaw("DELETE FROM Customers;");
        }

        // Optional: Wrap a test in a transaction and roll it back
        public static IDbContextTransaction BeginTransaction(NinjaLedgerDbContext context)
        {
            return context.Database.BeginTransaction();
        }
    }
}