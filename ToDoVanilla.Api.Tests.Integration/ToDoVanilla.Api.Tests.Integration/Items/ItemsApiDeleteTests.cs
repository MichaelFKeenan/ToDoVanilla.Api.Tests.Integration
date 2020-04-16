using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Items
{
    public class ItemsApiDeleteTests
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

        class ItemToDelete
        {
            public ItemToDelete(int _Id)
            {
                Id = _Id;
            }
            public int Id { get; }
        }

        [Test]
        public async Task DeleteItem()
        {
            client.DefaultRequestHeaders.Accept.Clear();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, ApiUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ItemToDelete(1)), Encoding.UTF8, "application/json")
            };

            var result = await client.SendAsync(request);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            var itemInDb = await _DataManagement.GetItemById(1);
            Assert.IsNull(itemInDb);
        }
    }
}
