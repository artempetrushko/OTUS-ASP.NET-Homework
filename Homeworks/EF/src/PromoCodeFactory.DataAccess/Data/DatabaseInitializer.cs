namespace PromoCodeFactory.DataAccess.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(DatabaseContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            dbContext.Employees.AddRange(FakeDataFactory.Employees);
            dbContext.Preferences.AddRange(FakeDataFactory.Preferences);
            dbContext.Customers.AddRange(FakeDataFactory.Customers);

            dbContext.SaveChanges();
        }
    }
}
