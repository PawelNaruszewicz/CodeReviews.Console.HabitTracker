namespace HabitTracker
{
    using Microsoft.Data.Sqlite;
    internal class Program
    {
        static void Main(string[] args)
        {
            using SqliteConnection? connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                            CREATE TABLE user (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL
                );

                INSERT INTO user
                VALUES (1, 'Brice'),
                       (2, 'Alexander'),
                       (3, 'Nate');
                """;

            command.ExecuteNonQuery();

        }
    }
}
