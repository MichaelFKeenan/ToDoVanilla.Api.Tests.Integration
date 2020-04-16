using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;
using ToDoVanilla.Api.Tests.Integration.Categories;
using ToDoVanilla.Api.Tests.Integration.Items;

namespace ToDoVanilla.Api.Tests.Integration
{

    //connect on constructor and make a singleton
    public class DataManagement
    {
        const string connString = "Host=localhost;Port=5432;Username=postgres;Password=hwaaw488;Database=todo_test";

        public async Task SetUpTestData()
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

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

            conn.Close();
        } 
    
        public async Task<Item> GetItemById(int id)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            Item val = new Item();

            using (NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM public.items WHERE id = {id};", conn))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    val.Id = reader["id"].ToString();
                    val.Name = reader["name"].ToString();
                    val.CategoryId = reader["categoryId"].ToString();
                    val.Complete = bool.Parse(reader["complete"].ToString());
                    val.CompleteBy = reader["completeBy"].ToString();
                    val.Description = reader["description"].ToString();
                    val.Effort = int.Parse(reader["effort"].ToString());
                    val.Priority = int.Parse(reader["priority"].ToString());
                }
            }

            conn.Close();

            return val;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            Category val = new Category();

            using (NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM public.categories WHERE id = {id};", conn))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    val.Id = reader["id"].ToString();
                    val.Name = reader["name"].ToString();
                }
            }

            conn.Close();

            return val;
        }
    }
}
