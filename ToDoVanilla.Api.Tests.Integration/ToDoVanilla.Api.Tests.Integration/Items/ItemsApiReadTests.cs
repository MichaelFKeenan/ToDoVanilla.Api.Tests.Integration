using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Items
{
    public class ItemsApiReadTests
    {
        private const string ApiUrl = "http://localhost:8080/api/items/";

        private static readonly HttpClient client = new HttpClient();

        private DataManagement _DataManagement;

        [SetUp]
        public async Task SetUp()
        {
            _DataManagement = new DataManagement();
            await _DataManagement.SetUpTestData();
        }

        [Test]
        public async Task GetAllItemsWithCategoryInfo()
        {
            var expectedItems = new List<DisplayItem>()
            {
                new DisplayItem()
                {
                    Id = "1",
                    Name = "item 1",
                    Complete = false,
                    Priority = 5,
                    Description = "some desc",
                    Effort = 4,
                    CompleteBy = "2001-01-01T00:00:00.000Z",
                    CategoryId = "1",
                    CategoryName = "category 1"
                },
                new DisplayItem()
                {
                    Id = "2",
                    Name = "item 2",
                    Complete = false,
                    Priority = 5,
                    Description = "some desc",
                    Effort = 5,
                    CompleteBy = "2002-01-01T00:00:00.000Z",
                    CategoryId = "2",
                    CategoryName = "category 2"
                }
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.GetAsync(ApiUrl);

            var result = await stringTask;

            string jsonContent = result.Content.ReadAsStringAsync().Result;
            var serializedResult = JsonConvert.DeserializeObject<List<DisplayItem>>(jsonContent);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(serializedResult.Count, expectedItems.Count);
            Assert.AreEqual(serializedResult[0], expectedItems[0]);
            Assert.AreEqual(serializedResult[1], expectedItems[1]);
        }

        [Test]
        public async Task GetItem()
        {
            var expectedItem = new Item()
            {
                Id = "1",
                Name = "item 1",
                Complete = false,
                Priority = 5,
                Description = "some desc",
                Effort = 4,
                CompleteBy = "2001-01-01T00:00:00.000Z",
                CategoryId = "1"
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.GetAsync($"{ApiUrl}/getitem/1");

            var result = await stringTask;

            string jsonContent = result.Content.ReadAsStringAsync().Result;
            var serializedResult = JsonConvert.DeserializeObject<Item>(jsonContent);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(serializedResult, expectedItem);
        }
    }
}