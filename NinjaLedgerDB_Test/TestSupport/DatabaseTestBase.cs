using NinjaDB.Data;

namespace NinjaLedgerDB_Test.TestSupport
{
    public abstract class DatabaseTestBase
    {
        protected NinjaLedgerDbContext Context { get; }

        protected DatabaseTestBase()
        {
            Context = TestDb.GetContext();
            TestDb.Reset(Context);
        }
    }
}
