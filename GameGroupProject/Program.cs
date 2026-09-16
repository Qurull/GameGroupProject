using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        static readonly Random random = new();

        static readonly string[] gameItems = ["Hangman", "MineSweeper", "Jeopardy", "MasterMind"];

        static readonly ConsoleColor[] colors = [ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Yellow];

        static readonly ConsoleColor[] darkColors = [ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray];
       
        static readonly char[] chars = ['●', '○', '◆', '◈', '◇', '×', '◻', '◼'];

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            PrintMainMenu();
            Console.ReadKey(true);
        }

        static void PrintMainMenu()
        {
            Console.Title = "Arcade";
            Console.CursorVisible = false;

            PrintBackground();
            PrintGameTitle("Welcome to Arcade!", (Console.WindowWidth/2 - 24, 3), (16, 7));
            int selectedGame = PrintGameMenu(0, (0, Console.WindowHeight/2));
            PrintGameLoading($"Loading {gameItems[selectedGame]}");
            
            if (selectedGame == 0)
                return;
            else if (selectedGame == 1)
                return;
            else if (selectedGame == 2)
                return;
            else if (selectedGame == 3)
                return;
        }

        static void PrintBackground()
        {
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    if (random.NextDouble() > 0.05) 
                        continue;
                    Console.ForegroundColor = darkColors[random.Next(darkColors.Length)];
                    Console.SetCursorPosition(x, y);
                    Console.Write(chars[random.Next(chars.Length)]);
                    Console.ResetColor();
                }
            }
        }

        static void PrintGameTitle(string text, (int x, int y) position, (int x, int y) size)
        {
            for (int y = 0; y <= size.y; y++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    char character = ' ';
                    if (x == 0 || x == size.x || y == 0 || y == size.y)
                        character = chars[random.Next(chars.Length)];

                    Console.ForegroundColor = colors[random.Next(colors.Length)];
                    Console.SetCursorPosition(position.x + x * 3, position.y + y);
                    Console.Write(character + "  ");
                    Console.ResetColor();
                }
                Thread.Sleep(100);
            }
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < text.Length; i++)
            {
                Console.SetCursorPosition(position.x + (size.x * 2 - text.Length)/2 + i*2, position.y + size.y/2 + i % 2);
                Console.Write(text[i]);
                Thread.Sleep(50);
            }
            Console.ResetColor();
        }

        static int PrintGameMenu(int index, (int x, int y) position)
        {
            const int cardWidth = 16;
            const int spacing = 2;

            int totalWidth = gameItems.Length * cardWidth + (gameItems.Length - 1) * spacing;
            int startX = (Console.WindowWidth - totalWidth) / 2;
            int selectedItem = index;

            for (int i = 0; i < gameItems.Length; i++)
                PrintGameItem(i, selectedItem == i);

            while (true)
            {
                int previousItem = selectedItem;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                    selectedItem = selectedItem > 0 ? selectedItem - 1 : gameItems.Length - 1;
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                    selectedItem = (selectedItem + 1) % gameItems.Length;
                else if (keyInfo.Key == ConsoleKey.Enter)
                    break;

                PrintGameItem(previousItem, false);
                PrintGameItem(selectedItem, true);
            }

            void PrintGameItem(int index, bool selected)
            {
                string item = gameItems[index];
                int x = startX + index * (cardWidth + spacing);

                ConsoleColor color = selected ? colors[random.Next(colors.Length - 1)] : ConsoleColor.DarkGray;
                (int x, int y, int width, int height) gameItem = PrintBorder(null, color, color, (position.x + x, position.y + index % 2), (cardWidth, 4));
                
                Console.SetCursorPosition(x + (gameItem.width - item.Length) / 2 + 1, gameItem.y + gameItem.height / 2);
                Console.ForegroundColor = color;
                Console.WriteLine(item);
                Console.ResetColor();
            }

            return selectedItem;
        }

        static void PrintGameLoading(string text)
        {
            Console.Clear();
            PrintBackground();
            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth - text.Length - 3)/2, Console.WindowHeight/2);
                Console.Write($"{text}{new('.', i % 4)}   ");
                Thread.Sleep(200);
            }
        }

        static (int x, int y, int width, int height) PrintBorder(string title, ConsoleColor titleColor, ConsoleColor borderColor, (int x, int y) position, (int x, int y) size, bool particles = false, bool animate = false)
        {
            for (int y = 0; y <= size.y; y++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    char character = ' ';
                    if (x == 0 && y == 0) character = '┌';
                    else if (x == size.x && y == 0) character = '┐';
                    else if (x == 0 && y == size.y) character = '└';
                    else if (x == size.x && y == size.y) character = '┘';
                    else if (y == 0 || y == size.y) character = '─';
                    else if (x == 0 || x == size.x) character = '│';

                    bool spawnParticle = particles && random.NextDouble() <= 0.1 && x > 0 && x < size.x && y > 0 && y < size.y;
                    if (spawnParticle) character = '•';

                    Console.ForegroundColor = spawnParticle ? ConsoleColor.DarkGray : borderColor;
                    Console.SetCursorPosition(position.x + x, position.y + y);
                    Console.Write(character);
                    Console.ResetColor();
                }
                if (animate) Thread.Sleep(50);
            }
            if (title != null)
            {
                Console.ForegroundColor = titleColor;
                Console.SetCursorPosition(position.x + (size.x - (title.Length + 2)) / 2, position.y);
                Console.Write($" {title} ");
            }
            Console.ResetColor();

            return (position.x, position.y, size.x, size.y);
        }
    }
}
