using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Categories
{
    public class CategoriesApiReadTests
    {
        private const string ApiUrl = "http://localhost:8080/api/categories/";

        private static readonly HttpClient client = new HttpClient();

        private DataManagement _DataManagement;

        [SetUp]
        public async Task SetUp()
        {
            _DataManagement = new DataManagement();
            await _DataManagement.SetUpTestData();
        }

        [Test]
        public async Task GetAllCategories()
        {
            var expectedItems = new List<Category>()
            {
                new Category()
                {
                    Id = "1",
                    Name = "category 1"
                },
                new Category()
                {
                    Id = "2",
                    Name = "category 2"
                }
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.GetAsync(ApiUrl);

            var result = await stringTask;

            string jsonContent = result.Content.ReadAsStringAsync().Result;
            var serializedResult = JsonConvert.DeserializeObject<List<Category>>(jsonContent);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(serializedResult.Count, expectedItems.Count);
            Assert.AreEqual(serializedResult[0], expectedItems[0]);
            Assert.AreEqual(serializedResult[1], expectedItems[1]);
        }
    }
}