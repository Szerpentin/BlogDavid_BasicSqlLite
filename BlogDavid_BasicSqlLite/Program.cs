
using Microsoft.Data.Sqlite;

namespace BlogDavid_BasicSqlLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing db");
            string dbName = "blog.db";
            RemoveDBIfExists(dbName);
            string connectionString = CreateSqliteConnectionString(dbName);
            CreatePersonTable(connectionString);
            Console.WriteLine("Db created with person table");

            Console.WriteLine("Adding initial data");
            InsertPerson(connectionString, "Peter", 35);
            InsertPerson(connectionString, "Anna", 35);
            InsertPerson(connectionString, "David", 38);
            InsertPerson(connectionString, "Lilla", 27);

            Console.WriteLine("Listing persons");
            ListPersons(connectionString);

            Console.WriteLine("Deleting a person");
            DeletePerson(connectionString, 2);

            Console.WriteLine("Listing persons");
            ListPersons(connectionString);

            Console.WriteLine("Selecting person id = 1");
            SelectPerson(connectionString, 1);

            Console.ReadLine();
        }
        private static void RemoveDBIfExists(string dbName)
        {
            FileInfo fi = new FileInfo(dbName);
            if (fi.Exists)
            {
                fi.Delete();
            }
        }
        private static string CreateSqliteConnectionString(string dbName)
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
            builder.Add("Data Source", dbName);
            return builder.ConnectionString;
        }
        private static void CreatePersonTable(string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using(SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "CREATE TABLE person(id INTEGER PRIMARY KEY, full_name TEXT, age INT)";
                command.ExecuteNonQuery();
            }
        }

        private static void InsertPerson(string connectionString, string fullName, int age)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"INSERT INTO person (full_name, age) VALUES ('{fullName}', {age})";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void DeletePerson(string connectionString, int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"DELETE FROM person WHERE id = {id}";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void UpdatePerson(string connectionString, int id, string fullName, int age)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE person SET full_name = '{fullName}', age = {age} WHERE id = {id}";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void SelectPerson(string connectionString, int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT id, full_name, age FROM person WHERE id = {id}";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader.GetInt32(0)}, full_name: {reader.GetString(1)}, age: {reader.GetInt32(2)}");
                    }
                }
                connection.Close();
            }
        }

        private static void ListPersons(string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT id, full_name, age FROM person ORDER BY id DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader.GetInt32(0)}, full_name: {reader.GetString(1)}, age: {reader.GetInt32(2)}");
                    }
                }
                connection.Close();
            }
        }
    }
}