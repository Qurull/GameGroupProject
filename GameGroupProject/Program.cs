using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        static readonly string[] games = { "Hangman", "MineSweeper", "Jeopardy", "MasterMind" };

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            DisplayMainMenu();
        }

        /// <summary>
        /// Displays the title and the game selection menu,
        /// then displays a loading message for the selected game.
        /// </summary>
        static void DisplayMainMenu()
        {
            DisplayGameTitle();

            int selectedGame = SelectGame();
            Console.WriteLine($"Loading {games[selectedGame]} now...");
        }

        /// <summary>
        /// Displays the title in the console.
        /// </summary>
        static void DisplayGameTitle()
        {
            string title = "◈--- Welcome to Arcade! ---◈";
            for (int i = 0; i < title.Length; i++)
            {
                Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Yellow : ConsoleColor.Cyan;
                Console.Write(title[i]);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        /// <summary>
        /// Displays the game selection menu and allows the user to navigate
        /// through the available games using the Up and Down arrow keys.
        /// Pressing Enter selects the currently highlighted game.
        /// </summary>
        /// <returns>The index of the selected game in the games array.</returns>
        static int SelectGame()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.CursorVisible = false;

            int selectedIndex = 0;

            while (true)
            {
                DisplayGameMenu(selectedIndex, left, top);

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(selectedIndex - 1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(selectedIndex + 1, games.Length - 1);
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }

        /// <summary>
        /// Displays the game selection menu and highlights the currently selected game.
        /// </summary>
        /// <param name="selectedIndex">The index of the currently selected game.</param>
        /// <param name="left">Column position of the cursor, where the menu should start.</param>
        /// <param name="top">Row position of the cursor, where the menu should start.</param>
        static void DisplayGameMenu(int selectedIndex, int left, int top)
        {
            Console.SetCursorPosition(left, top);
            for (int i = 0; i < games.Length; i++)
            {
                bool selected = selectedIndex == i;
                Console.ForegroundColor = selected ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.WriteLine($"{(selected ? "◉" : "○")} {games[i]}");
            }
            Console.ResetColor();
        }
    }
}
