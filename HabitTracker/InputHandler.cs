using System.Globalization;

namespace HabitTracker
{
    internal class InputHandler
    {
        private const string enterDate = "Please insert the date. Format: dd-MM-yyyy. \nType 1 to use today's date \nType 0 to return to main menu";
        private const string invalidDate = "Invalid date. Format: dd-MM-yyyy. Type 0 to return to main menu or try again";
        private const string invalidNumber = "Invalid number. Try again";
        public string GetDateInput()
        {
            Console.WriteLine(enterDate);
            string? dateInput = Console.ReadLine();

            if (dateInput == "0") return "0";
            else if (dateInput == "1") dateInput = DateTime.Now.ToString("dd-MM-yyyy");

                while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                {
                    Console.WriteLine(invalidDate);
                    dateInput = Console.ReadLine();
                    if (dateInput == "0") return "0";
                }

            return dateInput;
        }
        public int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") return 0;

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine(invalidNumber);
                numberInput = Console.ReadLine();
                if (numberInput == "0") return 0;
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}