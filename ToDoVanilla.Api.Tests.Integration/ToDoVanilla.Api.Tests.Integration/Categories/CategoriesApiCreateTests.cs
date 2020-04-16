using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration.Categories
{
    public class CategoriesApiCreateTests
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
        public async Task CreateCategory()
        {
            var expectedItem = new Category()
            {
                Id = "3",
                Name = "new item"
            };

            var newItem = new Category()
            {
                Name = "new item"
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.PostAsJsonAsync(ApiUrl, newItem);

            var result = await stringTask;

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            var itemInDb = await _DataManagement.GetCategoryById(3);
            Assert.AreEqual(itemInDb, expectedItem);
        }
    }
}