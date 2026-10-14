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
        private int currentHabitId = 1;
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
            command.CommandText = $"DELETE from records WHERE Id = @recordId AND HabitId = @habitId";
            command.Parameters.AddWithValue("@recordId", recordId);
            command.Parameters.AddWithValue("@habitId", currentHabitId);

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
                $"INSERT INTO records(Date, QUANTITY, HabitId) VALUES (@date, @quantity, @habitId)";
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@habitId", currentHabitId);

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
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM records WHERE Id = @recordId AND HabitId = @habitId)";
            checkCommand.Parameters.AddWithValue("@recordId", recordId);
            checkCommand.Parameters.AddWithValue("@habitId", currentHabitId);

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
            command.CommandText = $"UPDATE records SET date = @date, quantity = @quantity WHERE Id = @recordId";
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
                $"SELECT * FROM records WHERE HabitId = @habitId";
            command.Parameters.AddWithValue("@habitId", currentHabitId);

            List<RecordModel> tableData = new List<RecordModel>();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new RecordModel
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
            foreach (RecordModel data in tableData)
            {
                Console.WriteLine($"{data.Id} - {data.Date.ToString("dd-MM-yyyy")} - Quantity: {data.Quantity}");
            }
            Console.WriteLine("---------------------------");
        }
        public void SeedRecordsData()
        {
            int numberOfRecordsToAdd = inputHandler.GetNumberInput("Input the number of records you want to add");
            if (numberOfRecordsToAdd == 0) return;

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            for (int i = 0; i < numberOfRecordsToAdd; i++)
            {
                DateTime start = new DateTime(1995, 1, 1);
                int range = (DateTime.Today - start).Days;
                string date = start.AddDays(Random.Shared.Next(range)).ToString("dd-MM-yyyy");

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $"INSERT INTO records(Date, QUANTITY, HabitId) VALUES (@date, @quantity, @habitId)";
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@quantity", Random.Shared.Next(0, 100));
                command.Parameters.AddWithValue("@habitId", currentHabitId);

                command.ExecuteNonQuery();
            }
            connection.Close();

        }
        public bool SelectCurrentHabit()
        {
            bool habitExists = false;
            while (!habitExists)
            {
                Console.Clear();

                using SqliteConnection? connection = new SqliteConnection(dataSource);
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Unit from habits";

                using SqliteDataReader reader = command.ExecuteReader();
                Console.WriteLine("Available habits");
                Console.WriteLine("-----------------");

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetInt32(0)} - {reader.GetString(1)} - {reader.GetString(2)}");
                }
                Console.WriteLine("-----------------");

                int inputId = inputHandler.GetNumberInput("Type the ID of the habit you want to select or 0 if you want to go back:");
                if (inputId == 0) return false;

                SqliteCommand checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT EXISTS (SELECT 1 FROM habits WHERE Id = @habitsId)";
                checkCommand.Parameters.AddWithValue("@habitsId", inputId);

                int exists = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (exists == 1)
                {
                    currentHabitId = inputId;
                    habitExists = true;
                    Console.WriteLine($"Habit {currentHabitId} selected successfully!");
                }
                else
                {
                    Console.WriteLine($"\nHabit with ID {inputId} does not exist. Try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return true;
        }
    }
}