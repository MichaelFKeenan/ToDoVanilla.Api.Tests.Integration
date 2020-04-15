using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoVanilla.Api.Tests.Integration
{
    public partial class Tests
    {
        private const string ApiUrl = "http://localhost:8080/api/items/";

        private static readonly HttpClient client = new HttpClient();

        [SetUp]
        public async Task SetUp()
        {
            var items = new List<string>();
            //obviously need to hide these login details
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=hwaaw488;Database=todo_test";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();
            try
            {
                //reset pks?
                await using (var cmd = new NpgsqlCommand("DELETE FROM public.items;", conn))
                {
                    cmd.ExecuteNonQuery();
                }; 
                await using (var cmd = new NpgsqlCommand("ALTER SEQUENCE items_id_seq RESTART WITH 1;", conn))
                {
                    cmd.ExecuteNonQuery();
                };
                await using (var cmd = new NpgsqlCommand("DELETE FROM public.categories;", conn))
                {
                    cmd.ExecuteNonQuery();
                };
                await using (var cmd = new NpgsqlCommand("ALTER SEQUENCE categories_id_seq RESTART WITH 1;", conn))
                {
                    cmd.ExecuteNonQuery();
                };

                await using (var cmd = new NpgsqlCommand($"INSERT INTO categories(name) VALUES('category 1'),('category 2'); ", conn))
                {
                    cmd.ExecuteNonQuery();
                };

                await using (var cmd = new NpgsqlCommand($"INSERT INTO items(name, complete, priority, \"categoryId\", description, effort, \"completeBy\") " +
                    $"VALUES('item 1', '0', 5, 1, 'some desc', 4,'01/01/2001'),('item 2', '0', 5, 2, 'some desc', 5,'01/01/2002'); ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Test]
        public async Task GetAllItemsWithCategoryInfo()
        {
            var expectedItems = new List<Item>()
            {
                new Item()
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
                new Item()
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
            var serializedResult = JsonConvert.DeserializeObject<List<Item>>(jsonContent);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(serializedResult[0], expectedItems[0]);
        }
    }
}