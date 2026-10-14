namespace HabitTracker
{
    using Microsoft.Data.Sqlite;

    internal class Program
    {
        private static Records records = new Records();
        private const string closeApplication = "Press 0 to Close Application.";
        private const string lines = "---------------------------";
        private const string dataSource = "Data Source=Habit-Tracker.db";

        static void Main(string[] args)
        {
            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Unit TEXT
                )";

            command.ExecuteNonQuery();

            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS records(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                QUANTITY INTEGER,
                HabitId INTEGER,
                FOREIGN KEY(HabitId) REFERENCES habits(id)
                )";

            command.ExecuteNonQuery();

            connection.Close();

            ChooseYourHabit();
        }
        public static void ChooseYourHabit()
        {
            Console.Clear();
            bool closeApp = false;

            while(!closeApp)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine(lines);
                Console.WriteLine("What would you like to do?");
                Console.WriteLine(closeApplication);
                Console.WriteLine("Press 1 to Select Current Habit.");
                Console.WriteLine("Press 2 to Create New Habit");
                Console.WriteLine(lines);

                string? commandInput = Console.ReadLine();

                switch(commandInput)
                {
                    case "0":
                        Console.WriteLine("Closing the application.");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        bool habitSelected = records.SelectCurrentHabit();
                        if (habitSelected) GetUserInput();
                        break;
                    case "2":
                        CreateHabit();
                        break;
                    default:
                        break;
                }
            }
        }
        public static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine("HABIT MENU");
                Console.WriteLine(lines);
                Console.WriteLine("What would you like to do?");
                Console.WriteLine(closeApplication);
                Console.WriteLine("Type 1 to View all Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("Type 5 to Seed Random Data");
                Console.WriteLine(lines + "\n");

                string? commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Closing the application.");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        records.ViewAllRecords();
                        pressAnyButton();
                        break;
                    case "2":
                        records.InsertRecord();
                        pressAnyButton();
                        break;
                    case "3":
                        records.DeleteRecord();
                        pressAnyButton();
                        break;
                    case "4":
                        records.UpdateRecord();
                        pressAnyButton();
                        break;
                    case "5":
                        records.SeedRecordsData();
                        pressAnyButton();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }
        private static void pressAnyButton()
        {
            Console.WriteLine("Press any button to continue");
            Console.ReadKey();
            Console.Clear();
        }
        public static void CreateHabit()
        {
            Console.Clear();
            Console.WriteLine("Enter the name of the habit");
            string nameOfTheHabit = Console.ReadLine();

            Console.WriteLine("Enter the unit of measure (glasses, minutes)");
            string measureUnit = Console.ReadLine();

            using SqliteConnection? connection = new SqliteConnection(dataSource);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO habits (Name, Unit) VALUES (@name, @unit)";
            command.Parameters.AddWithValue("@name", nameOfTheHabit);
            command.Parameters.AddWithValue("@unit", measureUnit);

            command.ExecuteNonQuery();
            Console.WriteLine("Habit added successfully!");
            connection.Close();
        }
    }
}
