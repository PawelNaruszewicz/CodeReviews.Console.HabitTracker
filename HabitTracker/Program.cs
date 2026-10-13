namespace HabitTracker
{
    using Microsoft.Data.Sqlite;
    using System.Globalization;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    internal class Program
    {
        static void Main(string[] args)
        {
            using SqliteConnection? connection = new SqliteConnection("Data Source=Habit-Tracker.db");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )
                ";

            command.ExecuteNonQuery();

            connection.Close();

            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();

            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("---------------------------");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type 0 to Close Application.");
                Console.WriteLine("Type 1 to View all Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("---------------------------\n");

                string? commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Closing the application.");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        ViewAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }
        private static void ViewAllRecords()
        {
            Console.Clear();

            using SqliteConnection? connection = new SqliteConnection("Data Source=Habit-Tracker.db");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new List<DrinkingWater>();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
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
        private static void DeleteRecord()
        {
            Console.Clear();
            ViewAllRecords();

            int recordId = GetNumberInput("Please type the Id of the record you want to delete or press 0 to go back to Main Menu");

            using SqliteConnection? connection = new SqliteConnection("Data Source=Habit-Tracker.db");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE from drinking_water WHERE Id ='{recordId}'";
            
            int rowCount = command.ExecuteNonQuery();

            if(rowCount ==0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exit.");
                DeleteRecord();
            }
            Console.WriteLine($"Record with {recordId} has been deleted");

            connection.Close();
            GetUserInput();
        }
        private static void UpdateRecord()
        {
            Console.Clear();
            ViewAllRecords();

            var recordId = GetNumberInput("Please type Id of the record you would liek to update. Type 0 to go back to Main menu");

            using SqliteConnection? connection = new SqliteConnection("Data Source=Habit-Tracker.db");
            connection.Open();

            SqliteCommand checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

            if(checkQuery ==0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exist.");
                connection.Close();
                UpdateRecord();
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("Plaese insert number of glasses or other measure of your choice(no decimals allowed");
            
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE drinking_water SET date ='{date}', quantity ={quantity} WHERE Id = {recordId}";
            
            command.ExecuteNonQuery();
            connection.Close();
        }
        private static void InsertRecord()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("Please insert number of glasses or other measure of your choice (no decimals allowed)");

            using SqliteConnection? connection = new SqliteConnection("Data Source=Habit-Tracker.db");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}', {quantity})";

            command.ExecuteNonQuery();

            connection.Close();
        }
        private static string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy. Type 0 to return to main menu");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                GetUserInput();
            }
            
            while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. Format: dd-MM-yy. Type 0 to return to main menu or try again");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }
        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            while(!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0) 
            {
                Console.WriteLine("Invalid number. Try again");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}
