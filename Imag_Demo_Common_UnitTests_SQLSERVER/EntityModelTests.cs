using Imag.Demo.Shared;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Northwind.Common.UnitTests.SqlServer
{
    public class EntityModelTests:IDisposable
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://host.docker.internal:80/") };

        public static class TestHelpers
        {
            private const string _jsonMediaType = "application/json";
            private const int _expectedMaxElapsedMilliseconds = 1000;
            private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

            public static async Task AssertResponseWithContentAsync<T>(Stopwatch stopwatch, HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode,T expectedContent)
            {
                AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
                Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
               // Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<T?>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions));
            }

            private static void AssertCommonResponseParts(Stopwatch stopwatch, HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(expectedStatusCode, response.StatusCode);
                Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds);
            }

            public static StringContent GetJsonStringContent<T>(T model)
                => new(JsonSerializer.Serialize(model), Encoding.UTF8, _jsonMediaType);

        }

        public void Dispose()
        {
           // _httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        }

        [Fact]
        public void DatabaseConnectTest()
        {
            string connstring = "Data Source = host.docker.internal,1433; User iD = sa; Password=Pass@word; Initial Catalog = ImagDB; TrustServerCertificate=True;";
            using (ImagDataContext db = new ImagDataContext(connstring))
            {
                Assert.True(db.Database.CanConnect());
            }
        }

        [Fact]
        public async Task GivenARequest_WhenCallingGetBooks_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            List<Customer> expectedContent = new()
            {      
            new Customer() {Id=1, Name = "Super Rolnicze Sp. Z o.o", Adress = "F. Żukowa", NIP = "5743631775", },
            new Customer() {Id=2, Name = "Kwiatosława & Leona Hyper Sp. Z o.o.", Adress = "Rozalii Rzewuskiej", NIP = "2245236044", },
            new Customer() {Id=3, Name = "SuperpofelaliHyper.com", Adress = "Irlandzka", NIP = "8598580610", },
            new Customer() {Id=4, Name = "Ultra krótkie Słodycze s.c", Adress = "Dalsza", NIP = "7331045371", },
            new Customer() {Id = 5,  Name = "Morzytagaty Paka s.c.", Adress = "Sabinówek", NIP = "6486911880", },
            };
            expectedContent.OrderBy(x=>x.Id).ToList();
            var stopwatch = Stopwatch.StartNew();

            // Act.
            
            List<Customer> actualContent = await  _httpClient.GetFromJsonAsync<List<Customer>>(_httpClient.BaseAddress + "api/Customers");
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "api/Customers");
            // Assert.
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
            //Assert.Equal(expectedContent, actualContent.OrderBy(x=>x.Id));
            Assert.True(expectedContent.Where(x => x.Id == 1).FirstOrDefault().NIP == actualContent.Where(x => x.Id == 1).FirstOrDefault().NIP);
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