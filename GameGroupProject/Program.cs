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
                LoadMasterMind();
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

        static (int x, int y, int width, int height) PrintBorder(string? title, ConsoleColor titleColor, ConsoleColor borderColor, (int x, int y) position, (int x, int y) size, bool particles = false, bool animate = false)
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

        static void LoadMasterMind()
        {
            const int ROW_SIZE = 5;
            const int COL_SIZE = 4;
            const int BEEP_FREQUENCY = 500;
            const int BEEP_DURATION = 80;

            ConsoleColor[] possibleColors = [
                ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Yellow
            ];
            ConsoleColor[,] colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];
            ConsoleColor[] secretColors = new ConsoleColor[COL_SIZE];

            char[] pins = ['○', '◔', '◑', '◕', '●'];
            int selectedRow, selectedCol;
            bool playerWon = false;

            LoadMasterMind((2, 1));

            void LoadMasterMind((int x, int y) position)
            {
                Console.Clear();
                Console.Title = "MasterMind";
                Console.CursorVisible = false;

                (int x, int y, int width, int height) border = PrintBorder("MasterMind", possibleColors[random.Next(possibleColors.Length)], ConsoleColor.White, position, (26, 12), true, true);
                string[] items = ["Play", "Guides", "Quit"];
                int selectedItem = 0;
                bool animated = true;

                while (true)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (animated) Thread.Sleep(100);

                        bool selected = selectedItem == i;
                        (int x, int y) = ((border.x + border.width / 2), (border.y + border.height / 2) - 1);
                        Console.SetCursorPosition(x - items[i].Length / 2 - 1, y + i * 2 - 1);
                        Console.ForegroundColor = selected ? possibleColors[random.Next(possibleColors.Length)] : ConsoleColor.White;
                        Console.WriteLine(selected ? $"● {items[i]} ●" : $"  {items[i]}  ");
                    }
                    if (animated) animated = false;
                    Console.ResetColor();

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                        selectedItem = int.Max(selectedItem - 1, 0);
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                        selectedItem = int.Min(selectedItem + 1, items.Length - 1);
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);
                        break;
                    }
                }

                if (selectedItem == 0)
                    StartGame((border.x, border.y));
                else if (selectedItem == 1)
                    PrintGameRuleMenu((border.x, border.y));
                else if (selectedItem == 2)
                    return;
            }

            void PrintGameRuleMenu((int x, int y) position)
            {
                Console.Clear();
                (int x, int y, int width, int height) = PrintBorder("Guide", ConsoleColor.Yellow, ConsoleColor.White, position, (110, 14), true, true);

                Console.SetCursorPosition(x + width / 2 - 7, y + 2);
                Console.WriteLine("● How to play ●");

                string[] description = [
                    "A secret sequence of 4 colors is randomly generated at the start of the game.",
                    "Use the Left and Right Arrow keys to choose a color.",
                    "Press Enter to place the selected color in your current row.",
                    "Fill all 4 spaces to complete your guess.",
                    "After completing a row, you will receive feedback showing how many colors are in the correct position.",
                    "You have 5 attempts to guess the secret sequence.",
                    "Try to figure out the correct sequence before you run out of attempts!"
                ];
                for (int i = 0; i < description.Length; i++)
                {
                    Console.SetCursorPosition(x + 2, y + 4 + i);
                    Console.WriteLine($"{1 + i}. {description[i]}");
                }

                Console.SetCursorPosition(x + width / 2 - 4, y + 4 + description.Length + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("● Go back ●");
                Console.ResetColor();

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                        break;
                }

                Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);
                LoadMasterMind((2, 1));
            }

            void StartGame((int x, int y) position)
            {
                Console.Clear();
                (int x, int y, int width, int height) = PrintBorder("MasterMind", possibleColors[random.Next(possibleColors.Length)], ConsoleColor.White, position, (27, 13), true, true);

                RestartGame();
                GenerateSecretColors();
                (int x, int y) board = PrintBoard((x + 5, y + 4));
                PrintColorSelectorMenu((x, y + height + 1), (width, 4), board);
            }

            void RestartGame()
            {
                colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];
                selectedRow = 0;
                selectedCol = 0;
                playerWon = false;
            }

            (int x, int y, int width, int height) PrintBorder(string title, ConsoleColor titleColor, ConsoleColor borderColor, (int x, int y) position, (int x, int y) size, bool particles = false, bool animate = false)
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

            (int x, int y) PrintBoard((int x, int y) position)
            {
                for (int row = 0; row < colorFields.GetLength(0); row++)
                {
                    PrintHighlighter(selectedRow == row ? '▸' : ' ', (position.x - 1, position.y + row));
                    for (int col = 0; col < colorFields.GetLength(1); col++)
                        Console.Write('○');
                    Console.WriteLine($" │ 🏳️: {pins[0]} 🚩: {pins[0]}");
                }

                Console.SetCursorPosition(position.x, position.y + colorFields.GetLength(0));
                for (int i = 0; i < 18; i++)
                    Console.Write('─');

                Console.SetCursorPosition(position.x, position.y + colorFields.GetLength(0) + 1);
                for (int i = 0; i < secretColors.Length; i++)
                    Console.Write('?');

                return position;
            }

            void PrintColorSelectorMenu((int x, int y) position, (int y, int x) size, (int x, int y) boardPosition)
            {
                PrintBorder("Guess a Color", ConsoleColor.Yellow, ConsoleColor.White, position, size, false, true);
                int selectedColorIndex = 0;

                while (!playerWon && selectedRow < ROW_SIZE)
                {
                    PrintColorOptions(selectedColorIndex, (position.x + 8, position.y + 2));

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                        selectedColorIndex = selectedColorIndex > 0 ? selectedColorIndex - 1 : possibleColors.Length - 1;
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        selectedColorIndex = (selectedColorIndex + 1) % possibleColors.Length;
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        PlaceColor(possibleColors[selectedColorIndex], boardPosition);
                        Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);

                        (int correctColorsCount, int incorrectColorsCount) = GetPinsByCurrentRow(selectedRow);
                        selectedCol++;

                        if (selectedCol >= COL_SIZE)
                        {
                            Console.SetCursorPosition(boardPosition.x + 11, boardPosition.y + selectedRow);
                            Console.Write(pins[correctColorsCount]);

                            Console.SetCursorPosition(boardPosition.x + 17, boardPosition.y + selectedRow);
                            Console.Write(pins[incorrectColorsCount]);
                            selectedCol = 0;

                            PrintHighlighter(' ', (boardPosition.x - 1, boardPosition.y + selectedRow));
                            selectedRow++;
                            PrintHighlighter('▸', (boardPosition.x - 1, boardPosition.y + selectedRow));
                        }
                        if (correctColorsCount == COL_SIZE)
                        {
                            playerWon = true;
                            break;
                        }
                    }
                }
                Console.Beep(1000, 300);
                PrintGameResult(boardPosition);
                PrintConfirmation(position);
            }

            void PrintColorOptions(int index, (int x, int y) position)
            {
                for (int i = 0; i < possibleColors.Length; i++)
                {
                    Console.ForegroundColor = possibleColors[i];
                    Console.SetCursorPosition(position.x + i * 2, position.y);
                    Console.Write('●');
                    Console.ResetColor();

                    Console.SetCursorPosition(position.x + i * 2, position.y + 1);
                    Console.Write(index == i ? '▲' : ' ');
                }
            }

            void GenerateSecretColors()
            {
                for (int i = 0; i < secretColors.Length; i++)
                    secretColors[i] = possibleColors[random.Next(possibleColors.Length)];
            }

            void RevealSecretColors((int x, int y) position)
            {
                for (int i = 0; i < secretColors.Length; i++)
                {
                    Console.ForegroundColor = secretColors[i];
                    Console.SetCursorPosition(position.x + i, position.y);
                    Console.WriteLine('●');
                }
                Console.ResetColor();
            }

            void PrintGameResult((int x, int y) position)
            {
                RevealSecretColors((position.x, position.y + ROW_SIZE + 1));

                Console.ForegroundColor = playerWon ? ConsoleColor.Green : ConsoleColor.Red;
                Console.SetCursorPosition(position.x + 5, position.y - 2);
                Console.WriteLine(playerWon ? "You win!" : "You lose!");
                Console.ResetColor();
            }

            void PrintConfirmation((int x, int y) position)
            {
                (int x, int y, int width, int height) = PrintBorder("Confirmation", ConsoleColor.Yellow, ConsoleColor.White, position, (27, 6), false, true);

                Console.SetCursorPosition(x + width / 2 - 9, y + 2);
                Console.WriteLine("Do you want a retry?");

                string[] items = ["Yes", "No"];
                int selectedItem = 0;

                while (true)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        Console.ForegroundColor = selectedItem == i ? ConsoleColor.White : ConsoleColor.DarkGray;
                        Console.SetCursorPosition(x + 8 + i * 8, y + 4);
                        Console.Write(items[i]);
                    }
                    Console.ResetColor();

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                        selectedItem = int.Max(selectedItem - 1, 0);
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        selectedItem = int.Min(selectedItem + 1, items.Length - 1);
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);
                        break;
                    }
                }

                if (selectedItem == 0)
                    StartGame((2, 1));
                else if (selectedItem == 1)
                    LoadMasterMind((2, 1));
            }

            void PlaceColor(ConsoleColor color, (int x, int y) position)
            {
                colorFields[selectedRow, selectedCol] = color;

                Console.ForegroundColor = color;
                Console.SetCursorPosition(position.x + selectedCol, position.y + selectedRow);
                Console.Write(pins[^1]);
                Console.ResetColor();
            }

            void PrintHighlighter(char character, (int x, int y) position)
            {
                Console.SetCursorPosition(position.x, position.y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(character);
                Console.ResetColor();
            }

            (int corrects, int incorrects) GetPinsByCurrentRow(int row)
            {
                int corrects = 0, incorrects = 0;
                bool[] used = new bool[secretColors.Length];

                for (int i = 0; i < secretColors.Length; i++)
                {
                    if (colorFields[row, i] == secretColors[i])
                    {
                        used[i] = true;
                        corrects++;
                    }
                }

                for (int i = 0; i < secretColors.Length; i++)
                {
                    if (colorFields[row, i] == secretColors[i])
                        continue;

                    for (int j = 0; j < secretColors.Length; j++)
                    {
                        if (!used[j] && colorFields[row, i] == secretColors[j])
                        {
                            incorrects++;
                            used[j] = true;
                            break;
                        }
                    }
                }

                return (corrects, incorrects);
            }
        }
    }
}
