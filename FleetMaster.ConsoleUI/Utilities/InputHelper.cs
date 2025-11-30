namespace FleetMaster.ConsoleUI.Utilities
{
    public static class InputHelper
    {
        public static string ReadString(string prompt)
        {
            Console.Write($"{prompt}: ");
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: Поле не може бути пустим.");
                Console.ResetColor();
                Console.Write($"{prompt}: ");
                input = Console.ReadLine();
            }
            return input;
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            int result;
            Console.Write($"{prompt} ({min}-{max}): ");
            while (!int.TryParse(Console.ReadLine(), out result) || result < min || result > max)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: Введіть коректне число.");
                Console.ResetColor();
                Console.Write($"{prompt}: ");
            }
            return result;
        }

        public static double ReadDouble(string prompt)
        {
            double result;
            Console.Write($"{prompt}: ");
            while (!double.TryParse(Console.ReadLine(), out result) || result < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: Введіть коректне додатнє число.");
                Console.ResetColor();
                Console.Write($"{prompt}: ");
            }
            return result;
        }
    }
}