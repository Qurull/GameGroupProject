using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        const int ROW_SIZE = 5; 
        const int COL_SIZE = 4;

        const int BEEP_FREQUENCY = 500;
        const int BEEP_DURATION = 80;

        static readonly Random random = new();

        static readonly char[] pins = ['○', '◔', '◑', '◕', '●'];

        static readonly ConsoleColor[] possibleColors = [
            ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, 
            ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Yellow
        ];

        static ConsoleColor[,] colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];

        static readonly ConsoleColor[] secretColors = new ConsoleColor[COL_SIZE];

        static int selectedRow, selectedCol;

        static bool playerWon = false;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "MasterMind";

            LoadMainMenu((2, 1));
            Console.ReadKey(true);
        }

        static void LoadMainMenu((int x, int y) position)
        {
            Console.Clear();
            (int x, int y, int width, int height) border = CreateBorder("MasterMind", possibleColors[random.Next(possibleColors.Length)], position, (26, 12), true);
            string[] items = ["Play", "Guides", "Quit"];
            int selectedItem = 0;

            while (true)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    bool selected = selectedItem == i;
                    (int x, int y) = ((border.x + border.width/2), (border.y + border.height/2) - 1);
                    Console.SetCursorPosition(x - items[i].Length/2 - 1, y + i * 2);
                    Console.ForegroundColor = selected ? possibleColors[random.Next(possibleColors.Length)] : ConsoleColor.White;
                    Console.WriteLine(selected ? $"● {items[i]} ●" : $"  {items[i]}  ");
                }
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
                LoadGameRuleMenu((border.x, border.y));
            else if (selectedItem == 2)
                return;
        }

        static void LoadGameRuleMenu((int x, int y) position)
        {
            Console.Clear();
            (int x, int y, int width, int height) = CreateBorder("Guide", ConsoleColor.Yellow, position, (110, 13), true);

            Console.SetCursorPosition(x + width/2 - 7, y + 2);
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

            Console.SetCursorPosition(x + width/2 - 4, y + 4 + description.Length + 1);
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
            LoadMainMenu((2, 1));
        }

        static void StartGame((int x, int y) position)
        {
            Console.Clear();
            (int x, int y, int width, int height) = CreateBorder("MasterMind", possibleColors[random.Next(possibleColors.Length)], position, (26, 12), true);

            RestartGame();
            GenerateSecretColors();
            (int x, int y) board = CreateBoard((x + 7, y + 4));
            CreateColorSelectorMenu((x, y + height + 2), (width, 3), board);
        }

        static void RestartGame()
        {
            colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];
            selectedRow = 0;
            selectedCol = 0;
            playerWon = false;
        }

        static (int x, int y, int width, int height) CreateBorder(string title, ConsoleColor color, (int x, int y) position, (int x, int y) size, bool particles = false)
        {
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(position.x, position.y + i);
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }

            Console.SetCursorPosition(position.x, position.y);
            Console.Write('┌');

            for (int i = 0; i < size.x; i++)
                Console.Write('─');

            Console.SetCursorPosition(position.x + size.x / 2 - title.Length / 2, position.y);
            Console.ForegroundColor = color;
            Console.Write($" {title} ");
            Console.ResetColor();

            Console.SetCursorPosition(position.x + size.x + 1, position.y);
            Console.Write('┐');

            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(position.x, position.y + i + 1);
                Console.Write('│');
            }
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(position.x + size.x + 1, position.y + i + 1);
                Console.Write('│');
            }

            Console.SetCursorPosition(position.x, position.y + size.y + 1);
            Console.Write('└');

            for (int i = 0; i < size.x; i++)
                Console.Write('─');
            Console.Write('┘');

            if (particles)
            {
                for (int i = 0; i < 24; i++)
                {
                    Console.SetCursorPosition(position.x + random.Next(size.x) + 1, position.y + random.Next(size.y) + 1);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write('•');
                }
                Console.ResetColor();
            }

            return (position.x, position.y, size.x, size.y);
        }

        static (int x, int y) CreateBoard((int x, int y) position)
        {
            for (int row = 0; row < colorFields.GetLength(0); row++)
            {
                PrintHighlighter(selectedRow == row ? '▸' : ' ', (position.x - 1, position.y + row));
                for (int col = 0; col < colorFields.GetLength(1); col++)
                    Console.Write('○');
                Console.WriteLine(" │ Pins: " + pins[0]);
            }

            Console.SetCursorPosition(position.x, position.y + colorFields.GetLength(0));
            for (int i = 0; i < 14; i++)
                Console.Write('─');

            Console.SetCursorPosition(position.x, position.y + colorFields.GetLength(0) + 1);
            for (int i = 0; i < secretColors.Length; i++)
                Console.Write('?');

            return position;
        }

        static void CreateColorSelectorMenu((int x, int y) position, (int y, int x) size, (int x, int y) boardPosition)
        {
            CreateBorder("Guess a Color", ConsoleColor.Yellow, position, size);
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

                    int pinCount = GetPinsByCurrentRow(selectedRow);
                    selectedCol++;

                    if (selectedCol >= COL_SIZE)
                    {
                        Console.SetCursorPosition(boardPosition.x + 13, boardPosition.y + selectedRow);
                        Console.Write(pins[pinCount]);
                        selectedCol = 0;

                        PrintHighlighter(' ', (boardPosition.x - 1, boardPosition.y + selectedRow));
                        selectedRow++;
                        PrintHighlighter('▸', (boardPosition.x - 1, boardPosition.y + selectedRow));
                    }
                    if (pinCount == COL_SIZE)
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

        static void PrintColorOptions(int index, (int x, int y) position)
        {
            for (int i = 0; i < possibleColors.Length; i++)
            {
                Console.SetCursorPosition(position.x + i * 2, position.y);
                Console.ForegroundColor = possibleColors[i];
                Console.Write('●');
                Console.ResetColor();

                Console.SetCursorPosition(position.x + i * 2, position.y + 1);
                Console.Write(index == i ? '▲' : ' ');
            }
        }

        static void GenerateSecretColors()
        {
            for (int i = 0; i < secretColors.Length; i++)
                secretColors[i] = possibleColors[random.Next(possibleColors.Length)];
        }

        static void RevealSecretColors((int x, int y) position)
        {
            for (int i = 0; i < secretColors.Length; i++)
            {
                Console.SetCursorPosition(position.x + i, position.y);
                Console.ForegroundColor = secretColors[i];
                Console.WriteLine('●');
            }
            Console.ResetColor();
        }

        static void PrintGameResult((int x, int y) position)
        {
            RevealSecretColors((position.x, position.y + ROW_SIZE + 1));

            Console.SetCursorPosition(position.x + 2, position.y - 2);
            Console.ForegroundColor = playerWon ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(playerWon ? "You win!" : "You lose!");
            Console.ResetColor();
        }

        static void PrintConfirmation((int x, int y) position)
        {
            (int x, int y, int width, int height) = CreateBorder("Confirmation", ConsoleColor.Yellow, position, (26, 5));

            Console.SetCursorPosition(x + width/2 - 9, y + 2);
            Console.WriteLine("Do you want a retry?");

            string[] items = ["Yes", "No"];
            int selectedItem = 0;

            while (true)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Console.SetCursorPosition(x + 8 + i * 8, y + 4);
                    Console.ForegroundColor = selectedItem == i ? ConsoleColor.White : ConsoleColor.DarkGray;
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
                LoadMainMenu((2, 1));
        }

        static void PlaceColor(ConsoleColor color, (int x, int y) position)
        {
            colorFields[selectedRow, selectedCol] = color;

            Console.SetCursorPosition(position.x + selectedCol, position.y + selectedRow);
            Console.ForegroundColor = color;
            Console.Write(pins[^1]);
            Console.ResetColor();
        }

        static void PrintHighlighter(char character, (int x, int y) position)
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(character);
            Console.ResetColor();
        }

        static int GetPinsByCurrentRow(int row)
        {
            int correctColorsCount = 0;

            for (int col = 0; col < secretColors.Length; col++)
                if (colorFields[row, col] == secretColors[col])
                    correctColorsCount++;

            return correctColorsCount;
        }
    }
}
