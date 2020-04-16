using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Items
{
    public class ItemsApiCreateTests
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
        public async Task CreateItem()
        {
            var expectedItem = new Item()
            {
                Id = "3",
                Name = "new item",
                Complete = false,
                Priority = 5,
                Description = "some desc",
                Effort = 4,
                CompleteBy = "01/01/2001 00:00:00",
                CategoryId = "1"
            };

            var newItem = new Item()
            {
                Name = "new item",
                Complete = false,
                Priority = 5,
                Description = "some desc",
                Effort = 4,
                CompleteBy = "01/01/2001 00:00:00",
                CategoryId = "1"
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.PostAsJsonAsync(ApiUrl, newItem);

            var result = await stringTask;

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            var itemInDb = await _DataManagement.GetItemById(3);
            Assert.AreEqual(itemInDb, expectedItem);
        }
    }
}