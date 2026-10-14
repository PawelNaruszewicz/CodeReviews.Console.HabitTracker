using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    internal class Records
    {
        private const string dataSource = "Data Source=Habit-Tracker.db";
        private const string idToUpdate = "Please type Id of the record you would like to update. Type 0 to go back to Main menu";
        private const string idToDelete = "Please type the Id of the record you want to delete or press 0 to go back to Main Menu";
        private const string numberOfGlasses = "Please insert number of glasses or other measure of your choice (no decimals allowed)";
        private InputHandler inputHandler;
        public Records()
        {
            inputHandler = new InputHandler();
        }
        public void DeleteRecord()
        {
            Console.Clear();
            ViewAllRecords();

            int recordId = inputHandler.GetNumberInput(idToDelete);
            if (recordId == 0) return;

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE from drinking_water WHERE Id = @recordId";
            command.Parameters.AddWithValue("@recordId", recordId);

            int rowCount = command.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exit.");
                DeleteRecord();
                return;
            }
            Console.WriteLine($"Record with {recordId} has been deleted");

            connection.Close();
        }
        public void InsertRecord()
        {
            string date = inputHandler.GetDateInput();
            if (date == "0") return;

            int quantity = inputHandler.GetNumberInput(numberOfGlasses);
            if (quantity == 0) return;

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES (@date, @quantity)";
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@quantity", quantity);

            command.ExecuteNonQuery();

            connection.Close();
        }
        public void UpdateRecord()
        {
            Console.Clear();
            ViewAllRecords();

            var recordId = inputHandler.GetNumberInput(idToUpdate);
            if (recordId == 0) return;

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = @recordId)";
            checkCommand.Parameters.AddWithValue("@recordId", recordId);

            int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exist.");
                connection.Close();
                UpdateRecord();
                return;
            }

            string date = inputHandler.GetDateInput();
            if (date == "0") return;

            int quantity = inputHandler.GetNumberInput(numberOfGlasses);
            if (quantity == 0) return;

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE drinking_water SET date = @date, quantity = @quantity WHERE Id = @recordId";
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@recordId", recordId);

            command.ExecuteNonQuery();
            connection.Close();
        }
        public void ViewAllRecords()
        {
            Console.Clear();

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new List<DrinkingWater>();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        });
                }
            }
            else
            {
                Console.WriteLine("No rows");
            }

            connection.Close();

            Console.WriteLine("---------------------------");
            foreach (DrinkingWater data in tableData)
            {
                Console.WriteLine($"{data.Id} - {data.Date.ToString("dd-MM-yyyy")} - Quantity: {data.Quantity}");
            }
            Console.WriteLine("---------------------------");
        }
    }
}