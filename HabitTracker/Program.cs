namespace HabitTracker
{
    using Microsoft.Data.Sqlite;

    internal class Program
    {
        private static Records records = new Records();
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

        public static void GetUserInput()
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
                Console.WriteLine("Type 5 to Seed Random Data");
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
    }
}
