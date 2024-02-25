using Imag.Demo.Shared;

namespace Northwind.Common.UnitTests.SqlServer
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectTest()
        {
            using (ImagDataContext db = new())
            {
                Assert.True(db.Database.CanConnect());
            }
        }
        /*
        [Fact]
        public void CategoryCountTest()
        {
            using (ImagDataContext db = new())
            {
                int expected = 8;
                int actual = db.Categories.Count();
                Assert.Equal(expected, actual);
            }
        }
        */
    }
}