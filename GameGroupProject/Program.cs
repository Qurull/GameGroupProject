using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializeMainMenu();
        }

        static void InitializeMainMenu()
        {
            ConsoleColor consoleColors = [ConsoleColor.Yellow, ConsoleColor.Red];
            string title = "◈--- Welcome to Arcade! ---◈";
            for (int i = 0; i < title.Length; i++)
            {

            }

            (int left, int top) = Console.GetCursorPosition();
            Console.CursorVisible = false;

            string[] options = ["Hangman", "MasterMind", "MineSweeper", "Jeopardy"];
            int optionIndex = 0;
            bool enterPressed = false;

            while (!enterPressed)
            {
                Console.SetCursorPosition(left, top);

                for (int i = 0; i < options.Length; i++)
                {
                    bool isMatching = optionIndex == i;
                    Console.ForegroundColor = isMatching ? ConsoleColor.Yellow : ConsoleColor.White;
                    Console.WriteLine($"{(isMatching ? "◉" : "○")} {options[i]}");
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        optionIndex = Math.Max(optionIndex - 1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        optionIndex = Math.Min(optionIndex + 1, options.Length - 1);
                        break;
                    case ConsoleKey.Enter:
                        enterPressed = true;
                        break;
                }
            }

            Console.ResetColor();
        }
    }
}
