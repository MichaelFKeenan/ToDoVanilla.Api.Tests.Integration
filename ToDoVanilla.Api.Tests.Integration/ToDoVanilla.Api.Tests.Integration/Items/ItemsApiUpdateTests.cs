using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Items
{
    public class ItemsApiUpdateTests
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
        public async Task UpdateItem()
        {
            var editedItem = new Item()
            {
                Id = "1",
                Name = "item 1 updated",
                Complete = true,
                Priority = 6,
                Description = "some edited desc",
                Effort = 4,
                CompleteBy = "01/01/2001 00:00:00",
                CategoryId = "2"
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.PutAsJsonAsync(ApiUrl, editedItem);

            var result = await stringTask;

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            var itemInDb = await _DataManagement.GetItemById(1);
            Assert.AreEqual(itemInDb, editedItem);
        }

        class CompleteItemRequest
        {
            public string completedItemId { get; set; }
            public bool isComplete { get; set; }
        }

        [Test]
        public async Task ToggleItemComplete()
        {
            var completeItemRequest = new CompleteItemRequest()
            {
                completedItemId = "1",
                isComplete = true
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.PutAsJsonAsync($"{ApiUrl}/toggleItemComplete", completeItemRequest);

            var result = await stringTask;

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            var itemInDb = await _DataManagement.GetItemById(1);
            Assert.IsTrue(itemInDb.Complete);
        }
    }
}
