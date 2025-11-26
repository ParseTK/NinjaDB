using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NinjaDB.Data;

namespace NinjaLedgerDB_Test.TestSupport
{
    public static class TestDb
    {
        // 1) Point this to your dedicated TEST database (not production!)
        private const string ConnectionString =
            "Server=DESKTOP-7UHN9DO\\MSSQLSERVER01;Database=NinjaLedgerDB_Test;Trusted_Connection=True;Encrypt=False;";

        // 2) Create a fresh DbContext
        public static NinjaLedgerDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<NinjaLedgerDbContext>()
                .UseSqlServer(ConnectionString)
                .Options;

            var context = new NinjaLedgerDbContext(options);

            // Optional: Ensure schema is present (use if you want tests to apply migrations automatically)
            // context.Database.Migrate();

            return context;
        }

        // 3) Reset database state before each test (order matters due to FKs)
        public static void Reset(NinjaLedgerDbContext context)
        {
            // Clear dependents first to avoid FK conflicts
            context.Database.ExecuteSqlRaw("DELETE FROM Orders;");
            context.Database.ExecuteSqlRaw("DELETE FROM Products;");
            context.Database.ExecuteSqlRaw("DELETE FROM Customers;");
        }

        // 4) Optional: Wrap a test in a transaction and roll it back
        public static IDbContextTransaction BeginTransaction(NinjaLedgerDbContext context)
        {
            return context.Database.BeginTransaction();
        }
    }
}
