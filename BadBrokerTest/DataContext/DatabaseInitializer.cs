using System.Data.Entity;

namespace BadBrokerTest.DataContext
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<TestDataContext>
    {
        protected override void Seed(TestDataContext context) {
            context.Database.Initialize(false);
            base.Seed(context);
        }
    }
}