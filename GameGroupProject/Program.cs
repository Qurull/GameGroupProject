using System.Diagnostics;
using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        static readonly Random random = new();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StartMainMenu();
        }

        static void StartMainMenu()
        {
            ConsoleColor[] colors = [ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Yellow];
            ConsoleColor[] darkColors = [ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray];

            string[] gameItems = ["Hangman", "MineSweeper", "Jeopardy", "MasterMind"];
            char[] chars = ['●', '○', '◆', '◈', '◇', '×', '◻', '◼'];

            PrintMainMenu();

            void PrintMainMenu()
            {
                while (true)
                {
                    Console.Clear();
                    Console.Title = "Arcade";
                    Console.CursorVisible = false;
                    Console.SetWindowSize(120, 30);
                    Console.SetCursorPosition(0, 0);

                    PrintBackground();
                    PrintGameTitle("Welcome to Arcade!", (Console.WindowWidth / 2 - 24, 3), (16, 7));
                    int selectedGame = PrintGameMenu(0, (0, Console.WindowHeight / 2));
                    PrintGameLoading($"Loading {gameItems[selectedGame]}");

                    Console.Clear();
                    Console.Title = $"Arcade - {gameItems[selectedGame]}";

                    if (selectedGame == 0)
                        LoadHangMan();
                    else if (selectedGame == 1)
                        LoadMineSweeper();
                    else if (selectedGame == 2)
                        LoadJeopardy();
                    else if (selectedGame == 3)
                        LoadMasterMind();
                }
            }

            void PrintBackground()
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

            void PrintGameTitle(string text, (int x, int y) position, (int x, int y) size)
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
                    Console.SetCursorPosition(position.x + (size.x * 2 - text.Length) / 2 + i * 2, position.y + size.y / 2 + i % 2);
                    Console.Write(text[i]);
                    Thread.Sleep(50);
                }
                Console.ResetColor();
            }

            int PrintGameMenu(int index, (int x, int y) position)
            {
                const int cardWidth = 16;
                const int spacing = 2;

                int totalWidth = gameItems.Length * cardWidth + (gameItems.Length - 1) * spacing;
                int startX = (Console.WindowWidth - totalWidth) / 2;
                int selectedItem = index;

                PrintCredits();

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

            static void PrintCredits()
            {
                string[] items = ["Jack", "Inge", "Mikkel", "Tjalfe"];
                string creditText = "Credits: " + string.Join(", ", items);
                Console.SetCursorPosition(Console.WindowWidth/2 - creditText.Length/2, Console.WindowHeight - 2);
                Console.Write(creditText);
            }

            void PrintGameLoading(string text)
            {
                Console.Clear();
                PrintBackground();
                for (int i = 0; i < 12; i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth - text.Length - 3) / 2, Console.WindowHeight / 2);
                    Console.Write($"{text}{new('.', i % 4)}   ");
                    Thread.Sleep(200);
                }
            }

            (int x, int y, int width, int height) PrintBorder(string? title, ConsoleColor titleColor, ConsoleColor borderColor, (int x, int y) position, (int x, int y) size, bool particles = false, bool animate = false)
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

            PrintMasterMindMenu((2, 1));

            void PrintMasterMindMenu((int x, int y) position)
            {
                Console.Clear();
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
                        Console.Write(selected ? $"● {items[i]} ●" : $"  {items[i]}  ");
                    }
                    Console.ResetColor();

                    // After the iteration, stops the animation, so it doesn't play in every iteration again...
                    if (animated) animated = false;

                    // We make sure selectedItem is clamped from minimum value to maximum value as it's length.
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
                Console.Write("● How to play ●");

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
                    Console.Write($"{1 + i}. {description[i]}");
                }

                Console.SetCursorPosition(x + width / 2 - 4, y + 4 + description.Length + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("● Go back ●");
                Console.ResetColor();

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                        break;
                }

                Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);
                PrintMasterMindMenu((2, 1));
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
                        if (x == 0 && y == 0)                character = '┌'; // Top left corner.
                        else if (x == size.x && y == 0)      character = '┐'; // Top right corner.
                        else if (x == 0 && y == size.y)      character = '└'; // Bottom left corner.
                        else if (x == size.x && y == size.y) character = '┘'; // Bottom right corner.
                        else if (y == 0 || y == size.y)      character = '─'; // Top-left -> Bottom-left and Top-right -> Bottom-right.
                        else if (x == 0 || x == size.x)      character = '│'; // Top-left -> Top-right and Bottom-left -> Bottom-right.

                        bool spawnParticle = particles && random.NextDouble() <= 0.1 && x > 0 && x < size.x && y > 0 && y < size.y;
                        if (spawnParticle) character = '•';

                        Console.SetCursorPosition(position.x + x, position.y + y);
                        Console.ForegroundColor = spawnParticle ? ConsoleColor.DarkGray : borderColor;
                        Console.Write(character);
                        Console.ResetColor();
                    }
                    if (animate) Thread.Sleep(50);
                }
                if (title != null)
                {
                    Console.SetCursorPosition(position.x + (size.x - (title.Length + 2)) / 2, position.y);
                    Console.ForegroundColor = titleColor;
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
                    Console.Write($" │ 🏳️: {pins[0]} 🚩: {pins[0]}");
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

                        (int blackPins, int whitePins) = GetPinsByCurrentRow(selectedRow);
                        selectedCol++;

                        if (selectedCol >= COL_SIZE)
                        {
                            Console.SetCursorPosition(boardPosition.x + 11, boardPosition.y + selectedRow);
                            Console.Write(pins[blackPins]);

                            Console.SetCursorPosition(boardPosition.x + 17, boardPosition.y + selectedRow);
                            Console.Write(pins[whitePins]);
                            selectedCol = 0;

                            PrintHighlighter(' ', (boardPosition.x - 1, boardPosition.y + selectedRow));
                            selectedRow++;
                            PrintHighlighter('▸', (boardPosition.x - 1, boardPosition.y + selectedRow));
                        }
                        if (blackPins == COL_SIZE)
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
                    Console.SetCursorPosition(position.x + i * 2, position.y);
                    Console.ForegroundColor = possibleColors[i];
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
                    Console.SetCursorPosition(position.x + i, position.y);
                    Console.ForegroundColor = secretColors[i];
                    Console.Write('●');
                }
                Console.ResetColor();
            }

            void PrintGameResult((int x, int y) position)
            {
                RevealSecretColors((position.x, position.y + ROW_SIZE + 1));

                Console.SetCursorPosition(position.x + 5, position.y - 2);
                Console.ForegroundColor = playerWon ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write(playerWon ? "You win!" : "You lose!");
                Console.ResetColor();
            }

            void PrintConfirmation((int x, int y) position)
            {
                (int x, int y, int width, int height) = PrintBorder("Confirmation", ConsoleColor.Yellow, ConsoleColor.White, position, (27, 6), false, true);

                Console.SetCursorPosition(x + width / 2 - 9, y + 2);
                Console.Write("Do you want a retry?");

                string[] items = ["Yes", "No"];
                int selectedItem = 0;

                while (true)
                {
                    // Iterating through the items and draws them onto the console.
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
                    PrintMasterMindMenu((2, 1));
            }

            void PlaceColor(ConsoleColor color, (int x, int y) position)
            {
                colorFields[selectedRow, selectedCol] = color;

                Console.SetCursorPosition(position.x + selectedCol, position.y + selectedRow);
                Console.ForegroundColor = color;
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

            (int blackPins, int whitePins) GetPinsByCurrentRow(int row)
            {
                bool[] usedColors = new bool[secretColors.Length];
                int blackPins = 0, whitePins = 0;

                // Checking for black pins: If color and position is matching with in secret colors.
                for (int i = 0; i < secretColors.Length; i++)
                {
                    if (colorFields[row, i] != secretColors[i])
                        continue;

                    usedColors[i] = true;
                    blackPins++;
                }

                // Checking for white pins: If color is correct, but it's at the wrong position.
                for (int i = 0; i < secretColors.Length; i++)
                {
                    if (colorFields[row, i] == secretColors[i])
                        continue;

                    for (int j = 0; j < secretColors.Length; j++)
                    {
                        if (!usedColors[j] && colorFields[row, i] == secretColors[j])
                        {
                            usedColors[j] = true;
                            whitePins++;
                            break;
                        }
                    }
                }

                return (blackPins, whitePins);
            }
        }
    
        static void LoadMineSweeper()
        {
            (string name, int gridSizeX, int gridSizeY, int bombs)[] difficulties = new (string, int, int, int)[]
            {
                ("Easy", 9, 9, 10),
                ("Medium", 16, 16, 40),
                ("Hard", 30, 16, 99),
                ("Extreme", 40, 30, 400)
            };
            ConsoleColor selectorColor = ConsoleColor.White;
            ConsoleColor headlineColor = ConsoleColor.DarkRed;
            bool gridGenerated = false;
            bool playerDied = false;
            char bombChar = '*';
            char[] possibleFlagChars = { 'F', 'P', 'X', '!', '?', '¤', bombChar, '#', 'O' };
            int selectedFlagChar = 0;
            int gridSizeX = 0;
            int gridSizeY = 0;
            int bombs = 0;
            int bombsMinusFlags = 0;
            char[,] grid = new char[,] { };
            bool[,] isRevealed = new bool[,] { };
            bool[,] flagPlaced = new bool[,] { };
            (int x, int y) cursor = (0, 0);
            bool timerIsRunning = false;
            int elapsedTime = 0;
            string currentSmiley = ":)";
            long gameStartTime = 0;
            int lastDisplayedTime = -1;

            Console.Clear();
            Console.CursorVisible = false;

            while (MinesweeperMenu())
            {
                StartGame();
            }

            bool MinesweeperMenu() // Returns true if the player starts the game, false if the player quits.
            {
                // Don't looks at this code pls :(
                // I was rushing getting through the menu, so this is not good code, but at least it works...
                Console.SetWindowSize(35, 15);
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = headlineColor;
                    string title = "M I N E S W E E P E R !";
                    string padding = new string(' ', (Console.WindowWidth - title.Length) / 2);
                    Console.WriteLine($"\n{padding + title}");
                    PrintMenuItem(0, true);
                    PrintMenuItem(1);
                    PrintMenuItem(2);

                    int selectedIndex = 0;
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar)
                    {
                        PrintMenuItem(selectedIndex);
                        if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                        {
                            selectedIndex--;
                            selectedIndex = selectedIndex < 0 ? 2 : selectedIndex;
                        }
                        else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                        {
                            selectedIndex++;
                            selectedIndex = selectedIndex > 2 ? 0 : selectedIndex;
                        }
                        PrintMenuItem(selectedIndex, true);
                        key = Console.ReadKey(true);
                    }
                    if (key.Key == ConsoleKey.Escape) return false;

                    int menu = selectedIndex;

                    if (menu == 0)
                    {
                        for (int i = 0; i < difficulties.Length + 1; i++)
                        {
                            PrintMenuItem(i + 3, i == 1);
                        }
                        selectedIndex = 1;
                        key = new ConsoleKeyInfo();
                        while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar)
                        {
                            PrintMenuItem(selectedIndex + 3);
                            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                            {
                                selectedIndex--;
                                selectedIndex = selectedIndex < 0 ? difficulties.Length : selectedIndex;
                            }
                            else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                            {
                                selectedIndex++;
                                selectedIndex = selectedIndex > difficulties.Length ? 0 : selectedIndex;
                            }
                            PrintMenuItem(selectedIndex + 3, true);
                            key = Console.ReadKey(true);
                        }

                        if (key.Key != ConsoleKey.Escape)
                        {
                            if (selectedIndex > 0)
                            {
                                gridSizeX = difficulties[selectedIndex - 1].gridSizeX;
                                gridSizeY = difficulties[selectedIndex - 1].gridSizeY;
                                bombs = difficulties[selectedIndex - 1].bombs;
                            }
                            else
                            {
                                Console.SetCursorPosition(0, 2);
                                for (int i = 0; i < difficulties.Length * 2 + 2; i++)
                                {
                                    Console.WriteLine(new string(' ', Console.WindowWidth));
                                }
                                int newZero = difficulties.Length + 5; // +5 because these menu items are the 5th excluding preset difficulties in the PrintMenuItem function.
                                PrintMenuItem(newZero, true);
                                PrintMenuItem(newZero + 1);
                                PrintMenuItem(newZero + 2);

                                string[] customValues = new string[] { "", "", "" };

                                selectedIndex = newZero;
                                key = new ConsoleKeyInfo();
                                while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar)
                                {
                                    PrintMenuItem(selectedIndex, false, customValues[selectedIndex - newZero]);
                                    int keyInt = (int)key.Key;
                                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                                    {
                                        selectedIndex--;
                                        selectedIndex = selectedIndex < newZero ? newZero + 2 : selectedIndex;
                                    }
                                    else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                                    {
                                        selectedIndex++;
                                        selectedIndex = selectedIndex > newZero + 2 ? newZero : selectedIndex;
                                    }
                                    else if ((keyInt > 47 && keyInt < 58) || (keyInt > 95 && keyInt < 106))
                                    {
                                        // Pressed a number between 0 and 9.
                                        customValues[selectedIndex - newZero] += key.KeyChar;
                                    }
                                    else if (key.Key == ConsoleKey.Backspace && customValues[selectedIndex - newZero].Length > 0)
                                    {
                                        customValues[selectedIndex - newZero] = customValues[selectedIndex - newZero].Substring(0, customValues[selectedIndex - newZero].Length - 1);
                                    }

                                    PrintMenuItem(selectedIndex, true, customValues[selectedIndex - newZero]);
                                    key = Console.ReadKey(true);
                                }

                                // This covers most edge cases of stupid user input. One comes to mind that remains, though. If the user inputs something like 0000000000001, this code will think it is above the integer maximum, and adjust accordingly. This is stupid of course, but the code to patch this edge case is too much effort for how niche it is.
                                int gridSizeXMax = (Console.LargestWindowWidth - 6) / 2;
                                int gridSizeYMax = (Console.LargestWindowHeight - 2);
                                gridSizeX = customValues[0].Length == 0 ? difficulties[0].gridSizeX : customValues[0].Length > int.MaxValue.ToString().Length - 1 ? gridSizeXMax : Math.Min(int.Parse(customValues[0]), gridSizeXMax);
                                gridSizeY = customValues[1].Length == 0 ? difficulties[0].gridSizeY : customValues[1].Length > int.MaxValue.ToString().Length - 1 ? gridSizeYMax : Math.Min(int.Parse(customValues[1]), gridSizeYMax);
                                bombs = customValues[2].Length == 0 ? difficulties[0].bombs : customValues[2].Length > int.MaxValue.ToString().Length - 1 ? gridSizeX * gridSizeY - 1 : Math.Min(int.Parse(customValues[2]), gridSizeX * gridSizeY - 1);
                            }
                            if (key.Key != ConsoleKey.Escape)
                            {
                                return true;
                            }
                        }
                    }
                    else if (menu == 1)
                    {
                        int newZero = difficulties.Length + 8; // +8 because these menu items are the 8th excluding preset difficulties in the PrintMenuItem function.
                        PrintMenuItem(newZero, true);
                        PrintMenuItem(newZero + 1);
                        PrintMenuItem(newZero + 2);

                        selectedIndex = newZero;
                        key = new ConsoleKeyInfo();
                        while (key.Key != ConsoleKey.Escape && (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar))
                        {
                            PrintMenuItem(selectedIndex);
                            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                            {
                                selectedIndex--;
                                selectedIndex = selectedIndex < newZero ? newZero + 2 : selectedIndex;
                            }
                            else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                            {
                                selectedIndex++;
                                selectedIndex = selectedIndex > newZero + 2 ? newZero : selectedIndex;
                            }
                            else if (key.Key == ConsoleKey.A || key.Key == ConsoleKey.LeftArrow)
                            {
                                if (selectedIndex == newZero) selectedFlagChar = selectedFlagChar - 1 < 0 ? possibleFlagChars.Length - 1 : selectedFlagChar - 1;
                                if (selectedIndex == newZero + 1) headlineColor = (ConsoleColor)((int)headlineColor - 1 < 0 ? Enum.GetValues(typeof(ConsoleColor)).Length - 1 : (int)headlineColor - 1);
                                if (selectedIndex == newZero + 2) selectorColor = (ConsoleColor)((int)selectorColor - 1 < 0 ? Enum.GetValues(typeof(ConsoleColor)).Length - 1 : (int)selectorColor - 1);
                            }
                            else if (key.Key == ConsoleKey.D || key.Key == ConsoleKey.RightArrow)
                            {
                                if (selectedIndex == newZero) selectedFlagChar = selectedFlagChar + 1 > possibleFlagChars.Length - 1 ? 0 : selectedFlagChar + 1;
                                if (selectedIndex == newZero + 1) headlineColor = (ConsoleColor)((int)headlineColor + 1 > Enum.GetValues(typeof(ConsoleColor)).Length - 1 ? 0 : (int)headlineColor + 1);
                                if (selectedIndex == newZero + 2) selectorColor = (ConsoleColor)((int)selectorColor + 1 > Enum.GetValues(typeof(ConsoleColor)).Length - 1 ? 0 : (int)selectorColor + 1);
                            }

                            PrintMenuItem(selectedIndex, true);
                            key = Console.ReadKey(true);
                        }
                    }
                    else if (menu == 2)
                    {
                        return false;
                    }
                }
            }

            void PrintMenuItem(int type, bool selected = false, string customText = "")
            {
                // Type: 0 = Start, 1 = Settings, 2 = Quit, 3 = Custom difficulty, 4~X = Preset difficulties in order (X = # of difficulties + 4), X + 1 = Size X, X + 2 = Size Y, X + 3 = Bombs, X + 4 = Flag symbol, X + 5 = UI Colour, X + 6 = Cursor colour
                ConsoleColor color = selected ? selectorColor : selectorColor == ConsoleColor.Gray ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                Console.ForegroundColor = color;

                // This is garbage. It used to be a switch, then I needed the switch to do more than it could (compare to a non-constant), so changed to if-else. Can't be bothered to make this code not suck ass.
                if (type == 0)
                {
                    Console.SetCursorPosition(0, 3); // Start
                    Console.Write(" Start");
                }
                else if (type == 1)
                {
                    Console.SetCursorPosition(0, 5); // Settings
                    Console.Write(" Settings");
                }
                else if (type == 2)
                {
                    Console.SetCursorPosition(0, 7); // Quit
                    Console.Write(" Quit");
                }
                else if (type == 3)
                {
                    Console.SetCursorPosition(0, difficulties.Length * 2 + 3); // Custom difficulty
                    Console.Write(" Custom      "); // Extra spaces are there to erase the stuff that was printed on the line before
                }
                // Between 4 and difficulties.Length + 3 (inclusive)
                else if (type < difficulties.Length + 4) // Preset difficulties
                {
                    Console.SetCursorPosition(0, (type - 4) * 2 + 3);
                    Console.Write($" {difficulties[type - 4].name + new string(' ', Console.WindowWidth - difficulties[type - 4].name.Length)}");
                }
                else if (type == difficulties.Length + 5) // Custom size X
                {
                    Console.SetCursorPosition(0, 3);
                    Console.Write($" Size X: {customText} ");
                }
                else if (type == difficulties.Length + 6) // Custom size Y
                {
                    Console.SetCursorPosition(0, 5);
                    Console.Write($" Size Y: {customText} ");
                }
                else if (type == difficulties.Length + 7) // Custom bombs
                {
                    Console.SetCursorPosition(0, 7);
                    Console.Write($" Bombs: {customText} ");
                }
                else if (type == difficulties.Length + 8) // Settings custom flag symbol
                {
                    Console.SetCursorPosition(0, 3);
                    Console.Write($" Flag Symbol: {(selected ? "< " : "") + possibleFlagChars[selectedFlagChar] + (selected ? " >" : new string(' ', Console.WindowWidth - 15))}");
                }
                else if (type == difficulties.Length + 9) // Settings custom UI colour
                {
                    Console.ForegroundColor = headlineColor;
                    Console.SetCursorPosition(0, 0);
                    string title = "M I N E S W E E P E R !";
                    string padding = new string(' ', (Console.WindowWidth - title.Length) / 2);
                    Console.WriteLine($"\n{padding + title}");

                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(0, 5);
                    Console.Write($" UI Colour: {(selected ? "< " : "")}");
                    Console.ForegroundColor = headlineColor;
                    Console.Write(headlineColor);
                    Console.ForegroundColor = color;
                    Console.Write(selected ? " >   " : new string(' ', Console.WindowWidth - 12 - headlineColor.ToString().Length));
                }
                else if (type == difficulties.Length + 10) // Settings custom cursor colour
                {
                    Console.SetCursorPosition(0, 7);
                    Console.Write($" Cursor Colour: {(selected ? "< " : "")}");
                    Console.ForegroundColor = selectorColor;
                    Console.Write(selectorColor + (selected ? " >   " : new string(' ', Console.WindowWidth - 16 - selectorColor.ToString().Length)));
                    Console.ForegroundColor = color;
                }
                Console.ResetColor();
            }

            void StartGame()
            {
                // Set variables
                ResetVariables();

                // Set up console
                Console.SetWindowSize(gridSizeX * 2, gridSizeY + 2);
                Console.Clear();

                // Print the game out
                PrintHeadline();
                PrintEmptyGrid();

                // For some reason the console really wants to display the line just below the bottom of the window when I write to the console for the first time.
                // I couldn't find a way to fix the issue, but I solved it by just resizing the window to one bigger on the y-axis than it needs to be, and then resizing it back to normal afterwards.
                Console.SetWindowSize(gridSizeX * 2, gridSizeY + 1);
                while (!playerDied && !PlayerWon())
                {
                    UpdateGameTimer();

                    if (!Console.KeyAvailable)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.Enter)
                    {
                        RevealSpot(cursor.x, cursor.y);
                        PrintChar(cursor.x, cursor.y, true);
                        continue;
                    }

                    if (key.Key == (ConsoleKey)possibleFlagChars[selectedFlagChar] || key.Key == ConsoleKey.F)
                    {
                        ChangeFlag(cursor.x, cursor.y);
                        continue;
                    }

                    PrintChar(cursor.x, cursor.y);
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) cursor = (cursor.x, Math.Max(cursor.y - 1, 0));
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A) cursor = (Math.Max(cursor.x - 1, 0), cursor.y);
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) cursor = (cursor.x, Math.Min(cursor.y + 1, grid.GetLength(1) - 1));
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D) cursor = (Math.Min(cursor.x + 1, grid.GetLength(0) - 1), cursor.y);
                    PrintChar(cursor.x, cursor.y, true);
                    if (!playerDied && !PlayerWon())
                    {
                        if (random.Next(31) == 0) // 1/X chance of randomly changing the smiley for the sake of funny. X is the number passed to the Next method minus 1.
                        {
                            PrintHeadline("", true);
                        }
                    }
                }
                if (playerDied)
                {
                    PrintHeadline("X(", true);
                    Thread.Sleep(250);
                    // Reveal the whole grid
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            if (!isRevealed[i, j]) RevealSpot(i, j);
                        }
                    }

                    playerDied = false;
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else
                {
                    PrintHeadline(Console.WindowWidth > 25 ? ":D <( Good Job! )" : ":D", true);
                    Thread.Sleep(500);
                    ConsoleKey key = Console.ReadKey(true).Key;
                    while (key != ConsoleKey.W && key != ConsoleKey.A && key != ConsoleKey.S && key != ConsoleKey.D && key != ConsoleKey.UpArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.RightArrow) key = Console.ReadKey(true).Key;
                }
            }

            void PrintHeadline(string smiley = ":)", bool changeSmiley = false) // If newSmiley is empty, I will consider that wanting to make it random
            {
                Console.SetCursorPosition(0, 0);
                Console.BackgroundColor = headlineColor;
                Console.ForegroundColor = (int)headlineColor < 9 ? ConsoleColor.White : ConsoleColor.Black;
                if (smiley.Length == 0)
                {
                    // Choose random fun smiley
                    string[] smileys = new string[]
                    {
                    ":/", ":P", "xD", ":I", ";)", ":o", ":>", "x)", ":3", "(:", " :)", ":) ", ":-)", ">:)", ">xP", ">:O", ":U", ":T", ":c", "c:", ":3c", "B)", "OwO", "O-o", ">-O",":x", ";-;"
                    };
                    smiley = smileys[random.Next(smileys.Length)];
                    currentSmiley = smiley;
                }
                else if (!changeSmiley) smiley = currentSmiley;
                int bombCounterSize = bombsMinusFlags.ToString().Length;
                bombCounterSize = bombCounterSize <= gridSizeX * 2 ? bombCounterSize : gridSizeX * 2;
                bombCounterSize = Math.Max(gridSizeX > 1 ? 3 : 2, bombCounterSize);
                bool displayTimer = gridSizeX * 2 - bombCounterSize > 3;
                bool displaySmiley = gridSizeX * 2 - bombCounterSize > 3 + (changeSmiley ? smiley.Length + 1 : currentSmiley.Length + 1);
                int padSize = Console.WindowWidth - bombCounterSize - (displayTimer ? 3 : 0) - (displaySmiley ? smiley.Length : 0);
                string pad = new string(' ', padSize / 2); // Padding between stuff in the menu bar of the game.
                Console.Write($"\x1B[4m{(bombsMinusFlags.ToString().Length > bombCounterSize ? new string('9', bombCounterSize) : bombsMinusFlags.ToString()).PadLeft(bombCounterSize, '0') + pad + (displaySmiley ? changeSmiley ? smiley : currentSmiley : "") + (padSize % 2 != 0 ? " " : "") + pad + (displayTimer ? elapsedTime > 999 ? "999" : elapsedTime.ToString().PadLeft(3, '0') : "")}\x1B[0m");
                if (!gridGenerated) Console.Write("\n");
            }

            void PrintEmptyGrid()
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        PrintChar(i, j, i == cursor.x && j == cursor.y);
                    }
                    if (i != grid.GetLength(0) - 1) Console.Write("\n");
                }
            }
            void PrintChar(int x, int y, bool isSelected = false)
            {
                Console.SetCursorPosition(x * 2, y + 1);

                // Print flag or empty square if not revealed.
                if (!isRevealed[x, y])
                {
                    Console.BackgroundColor = isSelected ? selectorColor : ConsoleColor.Gray;
                    if (flagPlaced[x, y])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(possibleFlagChars[selectedFlagChar]);
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                    return;
                }

                // Decide on colour
                ConsoleColor inverseOfSelector = (ConsoleColor)Math.Abs((int)selectorColor - 15); // If white, is black, if black is white. Also works with other colours.
                Console.BackgroundColor = isSelected ? selectorColor : inverseOfSelector;
                switch (grid[x, y])
                {
                    default: // Case 0
                        Console.ForegroundColor = isSelected ? selectorColor : inverseOfSelector;
                        break;
                    case '1':
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case '2':
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case '3':
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case '4':
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;
                    case '5':
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case '6':
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case '7':
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                    case '8':
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case '*':
                        Console.ForegroundColor = isSelected ? inverseOfSelector : selectorColor;
                        break;
                }

                // Print character
                Console.Write(grid[x, y]);
                Console.Write(' ');
                Console.ResetColor();
            }

            void PopulateGrid(int bombs) // Put an amount of bombs specified by the bombs parameter randomly onto the field.
            {
                // Bombs will be distributed via a ticket system
                // Each index of the grid will draw a random number from the ticket array, which will then be removed from the array
                // When this is done, all numbers below a value specified by the bombs parameter will be made into bombs.

                int[] tickets = new int[grid.Length - 1]; // Create a ticket per grid slot, minus one for where the cursor is at the beginning of the game
                int[,] gridProxy = new int[grid.GetLength(0), grid.GetLength(1)]; // A stand-in for the grid array, with ints instead of chars

                // Create the tickets
                for (int i = 0; i < tickets.Length; i++)
                {
                    tickets[i] = i;
                }

                // Assign the tickets
                for (int i = 0; i < gridProxy.GetLength(0); i++)
                {
                    for (int j = 0; j < gridProxy.GetLength(1); j++)
                    {
                        // Skip this spot and assign the max value (to assure no bomb is spawned) if this is where the cursor is currently at.
                        if (i == cursor.x && j == cursor.y)
                        {
                            gridProxy[i, j] = gridProxy.Length - 1;
                            continue;
                        }

                        // Draw a random ticket and assign it in gridProxy
                        int randomIndex = random.Next(0, tickets.Length);
                        gridProxy[i, j] = tickets[randomIndex];

                        // Remove the drawn ticket from the tickets array (This functions similarly to List.RemoveAt()).
                        int[] updatedTickets = new int[tickets.Length - 1];
                        for (int k = 0; k < tickets.Length; k++)
                        {
                            if (k == randomIndex) continue;
                            updatedTickets[k < randomIndex ? k : k - 1] = tickets[k];
                        }
                        tickets = updatedTickets;
                    }
                }
                // Insert the bombs into the grid at the chosen spots
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        // If the drawn ticket number is less than the number of bombs to place, place a bomb in that position, otherwise place an empty space
                        if (gridProxy[i, j] < bombs) grid[i, j] = bombChar;
                        else grid[i, j] = ' ';
                    }
                }

                // Evaluate bomb adjacency and place corresponding numbers into the grid for later display purposes
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        // Positions below:
                        /*   x--->
                         * y 0 1 2
                         * | 3   4
                         * v 5 6 7
                         * 
                         *       -1     0     +1
                         *      x--------------->
                         * -1 y -1,-1  0,-1  1,-1
                         *    |
                         *    |
                         *  0 | -1, 0  0, 0  1, 0
                         *    |
                         *    |
                         * +1 v -1, 1  0, 1  1, 1
                         */

                        // Skip checking if this is a bomb
                        if (grid[x, y] == bombChar) continue;

                        // Check positions
                        int numberOfAdjacentBombs = 0;

                        if (x != 0 && y != 0 && grid[x - 1, y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 0 (-1,-1)
                        if (y != 0 && grid[x, y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 1 ( 0,-1)
                        if (x != grid.GetLength(0) - 1 && y != 0 && grid[x + 1, y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 2 ( 1,-1)

                        if (x != 0 && grid[x - 1, y] == bombChar) numberOfAdjacentBombs++; // Check pos 3 (-1, 0)
                        if (x != grid.GetLength(0) - 1 && grid[x + 1, y] == bombChar) numberOfAdjacentBombs++; // Check pos 4 (+1, 0)

                        if (x != 0 && y != grid.GetLength(1) - 1 && grid[x - 1, y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 5 (-1, 1)
                        if (y != grid.GetLength(1) - 1 && grid[x, y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 6 ( 0, 1)
                        if (x != grid.GetLength(0) - 1 && y != grid.GetLength(1) - 1 && grid[x + 1, y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 7 ( 1, 1)

                        grid[x, y] = (char)(numberOfAdjacentBombs + 48); // Have to add 48 to convert the int to the proper ascii value before casting to char. Lookup table for reference: https://www.ascii-code.com/
                    }
                }
            }
            void RevealSpot(int x, int y, bool calledRecursively = false)
            {
                if (playerDied)
                {
                    isRevealed[x, y] = true;
                    PrintChar(x, y);
                    return;
                }

                // If this is the first reveal, populate the grid with bombs. Doing this will guarantee a safe spot on the cursor.
                if (!gridGenerated)
                {
                    PopulateGrid(bombs);
                    gridGenerated = true;
                    // Start timer
                    StartGameTimer();
                }

                // Do nothing if the player has marked this spot as a bomb
                if (flagPlaced[x, y]) return;

                // Do nothing if the spot is already revealed. Only skip if called recursively to allow for revealing spots adjacent to an already revealed spots if the correct amount of flags are placed.
                if (isRevealed[x, y] && calledRecursively) return;

                // Game ends if a bomb is revealed
                if (grid[x, y] == bombChar)
                {
                    playerDied = true;
                }

                int numberOfNeabyFlags = 0;
                for (int i = Math.Max(0, x - 1); i < Math.Min(isRevealed.GetLength(0), x + 2); i++)
                {
                    for (int j = Math.Max(0, y - 1); j < Math.Min(isRevealed.GetLength(1), y + 2); j++)
                    {
                        if (flagPlaced[i, j]) numberOfNeabyFlags++;
                    }
                }

                // Update reveal status and print the character
                isRevealed[x, y] = true;
                PrintChar(x, y);

                // Reveal adjacent spots if there are 0 bombs nearby, OR if the player has placed the right amount of adjacent flags and this function call is not recursive
                // The recursive check is done to avoid game-ending cascades caused by placing wrong flags.
                if (!(grid[x, y] == '0'))
                {
                    if (!((char)(numberOfNeabyFlags + 48) == grid[x, y] && !calledRecursively)) return;
                }

                for (int i = Math.Max(0, x - 1); i < Math.Min(grid.GetLength(0), x + 2); i++)
                {
                    for (int j = Math.Max(0, y - 1); j < Math.Min(grid.GetLength(1), y + 2); j++)
                    {
                        RevealSpot(i, j, true);
                    }
                }
            }

            void ChangeFlag(int x, int y) // Place or remove flag
            {
                // Do nothing if the spot is revealed
                if (isRevealed[x, y]) return;

                if (flagPlaced[x, y]) bombsMinusFlags++;
                else bombsMinusFlags--;
                flagPlaced[x, y] = !flagPlaced[x, y];

                PrintHeadline();
                PrintChar(x, y, true);
            }

            bool PlayerWon() // Check if the player has won
            {
                int score = 0;
                for (int i = 0; i < isRevealed.GetLength(0); i++)
                {
                    for (int j = 0; j < isRevealed.GetLength(1); j++)
                    {
                        if (isRevealed[i, j] && grid[i, j] != bombChar) score++;
                    }
                }

                return score == gridSizeX * gridSizeY - bombs;
            }

            void StartGameTimer()
            {
                gameStartTime = Stopwatch.GetTimestamp();
                elapsedTime = 0;
                lastDisplayedTime = -1;
                timerIsRunning = true;
            }

            void UpdateGameTimer()
            {
                // ChatGPT helped me cook this up.
                // I initially wanted to use a second thread to keep track of time, because I thought it would be simpler, it's just 1 variable to update on a fixed schedule after all.
                // Doing that introduced major bugs because the thread was unsafe though. So I looked for a new solution and found the StopWatch which turns out to work perfectly for this task.
                if (!timerIsRunning) return;

                // GetTimeStamp returns time passed in ticks. Dividng by frequency effectively turns the time passed in ticks into time passed in seconds, because the frequency is the ticks per second.
                int elapsedSeconds = (int)((Stopwatch.GetTimestamp() - gameStartTime) / (double)Stopwatch.Frequency);

                if (elapsedSeconds != lastDisplayedTime)
                {
                    elapsedTime = elapsedSeconds;
                    lastDisplayedTime = elapsedSeconds;

                    if (!playerDied && gridGenerated)
                    {
                        PrintHeadline(currentSmiley);
                    }
                }
            }

            void ResetVariables() // Small helper method that just resets a bunch of variables back to default values when the player restarts a game.
            {
                cursor = (0, 0);
                bombsMinusFlags = bombs;
                grid = new char[gridSizeX, gridSizeY];
                isRevealed = new bool[gridSizeX, gridSizeY];
                flagPlaced = new bool[gridSizeX, gridSizeY];
                elapsedTime = 0;
                gridGenerated = false;
                timerIsRunning = false;
                currentSmiley = ":)";
            }
        }

        static void LoadHangMan()
        {
            Console.Clear(); //rydder konsollen for tekst
            Console.CursorVisible = true;
            Console.WriteLine("Welcome to Hangman, what is the word?");//beder spilleren om at indtaste et ord som skal gættes

            string guessWord = Console.ReadLine() ?? "";//læser ordet som spilleren indtaster
            Console.SetCursorPosition(0, Console.CursorTop - 1); //flytter cursoren op en linje

            StringBuilder hiddenWord = new StringBuilder(new string('_', guessWord.Length));//gemmer ordet som spilleren skal gætte
            int life = 8;//antal forsøg spilleren har til at gætte
            if (guessWord.Contains(" "))
            {
                for (int i = 0; i < guessWord.Length; i++) //går igennem ordet som spilleren skal gætte
                {
                    if (guessWord[i] == ' ') //hvis der er et mellemrum i ordet som spilleren skal gætte
                    {
                        hiddenWord[i] = ' ';//gemmer mellemrum i hiddenWord
                    }
                }
            }

            Console.WriteLine("Guess the word, you have " + life + " tries.");//beder spilleren om at gætte ordet
            DisplayHangman(life);
            Console.WriteLine(hiddenWord.ToString());//viser spilleren hvor mange bogstaver der er i ordet som skal gættes

            while (life > 0) //så længe spilleren har liv tilbage køre spillet
            {
                bool correctGuess = false; //variabel som holder styr på om spilleren har gættet rigtigt
                string guess = Console.ReadLine() ?? "";//læser spilleren gæt

                if (guess.Length == 1)//hvis spilleren gætter et bogstav
                {
                    for (int i = 0; i < guessWord.Length; i++)//går igennem ordet som spilleren skal gætte
                    {
                        if (guessWord[i].ToString().ToLower() == guess[0].ToString().ToLower())//hvis spilleren gætter rigtigt
                        {
                            hiddenWord[i] = guess[0];//gemmer det rigtige gæt i hiddenWord
                            correctGuess = true;//sætter correctGuess til true
                        }
                    }
                    if (correctGuess)//hvis spilleren gættede rigtigt
                    {
                        Console.Clear();
                        Console.WriteLine("Good guess! You have " + life + " lives left. What is the next letter?");//beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    }
                    else //hvis spilleren gættede forkert
                    {
                        life -= 1; //trækker et liv fra spilleren
                        Console.Clear();
                        Console.WriteLine("That was wrong you have " + life + " lives left. What is the next letter?"); //beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString()); // viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                        Console.Beep(1000, 500); //spiller en lyd når spilleren gætter forkert

                    }
                }

                else if (guess.Length >= 1) //hvis spilleren gætter et ord
                {
                    for (int i = 0; i < guessWord.Length; i++)//går igennem ordet som spilleren skal gætte
                    {
                        for (int j = 0; j < guess.Length; j++)//går igennem spilleren gæt)
                        {
                            if (guessWord[i].ToString().ToLower() == guess[j].ToString().ToLower())
                            {
                                hiddenWord[i] = guess[j];//gemmer det rigtige gæt i hiddenWord
                                correctGuess = true;//sætter correctGuess til true
                            }
                        }

                    }

                    if (correctGuess) //hvis spilleren gættede rigtigt
                    {
                        Console.Clear();
                        Console.WriteLine("Some of the letters match! You have " + life + " lives left. What is the next letter?");//beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte

                    }
                    else //hvis spilleren gættede forkert
                    {
                        life -= 1;
                        Console.Clear();
                        Console.WriteLine("None of the letters match. You have " + life + " lives left. What is the next letter?"); //beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString()); // viser hvor mange bogstaver der er i ordet dem som er belvet
                        Console.Beep(1000, 500); //spiller en lyd når spilleren gætter forkert
                    }

                }

                if (hiddenWord.ToString().ToLower() == guessWord.ToLower()) //hvis spilleren har gættet ordet
                {
                    Console.WriteLine("You guessed the word!");
                    Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine() ?? "";
                    if (playAgain.ToLower() == "y")
                    {
                        LoadHangMan();
                    }
                    else if (playAgain.ToLower() == "n") // hvis spilleren ikke vil spille igen og vil tilbage til menuen
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
                else if (life <= 0) //hvis spilleren har mistet alle liv
                {
                    Console.WriteLine("You ran out of lives!");
                    Console.WriteLine("The word was: " + guessWord);
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine() ?? "";
                    if (playAgain.ToLower() == "y")
                    {
                        LoadHangMan();
                    }
                    else if (playAgain.ToLower() == "n") //hvis spilleren ikke vil spille igen og vil tilbage til menuen
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
            }

            void DisplayHangman(int life) //metoden som viser tegner den hangman som spilleren har gættet forkert
            {
                string[] stages =
                {
                    // final state: hele kroppen og boksen skuppes væk
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        💀
             🚪       \👕/
             🚪        👖
             🚪        / \
            🧱🧱    🪜    [--]
                    ",
                    // hoved, krop, begge arme og begge ben
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😱
             🚪       \👕/
             🚪        👖
             🚪        / \
            🧱🧱    🪜[--]
                    ",
                    // hoved, krop, begge arme og et ben
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😨
             🚪       \👕/
             🚪        👖
             🚪        / 
            🧱🧱    🪜[--]
                    ",
                    // hoved, krop og begge arme
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😧
             🚪       \👕/
             🚪        👖
             🚪      
            🧱🧱    🪜[--]
                    ",
                    // hoved, krop og en arm
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😦
             🚪       \👕
             🚪        👖
             🚪      
            🧱🧱    🪜[--]
                    ",
                    // hoved og krop
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        🙁
             🚪        👕
             🚪        👖
             🚪      
            🧱🧱    🪜[--]
                    ",
                    // hoved
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😐
             🚪     
             🚪     
             🚪     
            🧱🧱    🪜[--]
                    ",
                    // stolpe
                    @"
             💴💴💴💴💴
             🚪        |
             🚪      
             🚪     
             🚪      
             🚪      
            🧱🧱    🪜[--]
                    ",
                    
                    // initial empty state bakke og boks
                    @"
                          
                          
                          
                          
                          
                          
            🧱🧱    🪜[--]
                    ",
                };

                Console.WriteLine(stages[life]);
            }
        }

        static void LoadJeopardy()
        {
            while (true)
            {
                // clear console and reset colors
                Console.Clear();
                Console.ResetColor();

                // write out menu options
                Console.WriteLine("Welcome to Jeopardy!\n");
                Console.WriteLine("\t1: Play game");
                Console.WriteLine("\t2: Rules");
                Console.WriteLine("\t3: Credits");
                Console.WriteLine("\t4: Exit Jeopardy");

                // write out instructions
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nInput a number from the menu and press enter.");
                Console.ResetColor();

                // read input from player
                string menuChoice = Console.ReadLine() ?? "";

                // start the game
                if (menuChoice == "1") ManageRounds();

                // read the rules
                else if (menuChoice == "2")
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Jeopardy rules: \n");
                    Console.ResetColor();

                    // concept
                    Console.WriteLine("Jeopardy is a reversed quiz game where players receive the answers (clues) and then have to ask the correct questions to win points.\n");
                    Console.WriteLine("If they ask the wrong question - or forget to form their answer to the clue as a question (what is/who is/where is/etc.) - they lose points.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // amount of players and their buzzers
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Players: \n");
                    Console.ResetColor();

                    Console.WriteLine("Normally there are 3 players competing in a Jeopardy game, but in this version you can play with 1-3 players.\n\n");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Buzzers: \n");
                    Console.ResetColor();

                    Console.WriteLine("Each player gets assigned a buzzer. When a clue is revealed, whoever presses their buzzer first gets to answer with a question.\n");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\tPlayer 1 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("A key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tPlayer 2 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("L key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\tPlayer 3 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("G key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.WriteLine("\nIf you want to play the game comfortably, please sit according to your buzzer keys.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // the different rounds
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Different Rounds: \n");
                    Console.ResetColor();


                    Console.WriteLine("There are 3 rounds of Jeopardy: Jeopardy, Double Jeopardy and Final Jeopardy.");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Jeopardy, the players are presented with a board of 6 categories with 5 clues each - the clues are represented by the amount of points they're worth (200-1000 points).\n");
                    Console.WriteLine("One of the players are asked to choose a category and a clue.");
                    Console.WriteLine("After this the clue is revealed and the players can buzz in to answer with the, hopefully, correct question. If their answer is incorrect the other players will once again have a chance to answer.\n");
                    Console.WriteLine("Each clue can only be chosen once, and when there are no more clues the round ends.");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Double Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Double Jeopardy, the players are presented with a similar board and the same rules as in Jeopardy - but this time the clues are worth more points (400-2000).");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Final Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Final Jeopardy, the rules are different. And only players who have gathered more than 0 points can participate in this round.\n");
                    Console.WriteLine("A random category is chosen and each player wages an amount of their points - at least 1 point, at most all their points.");
                    Console.WriteLine("The final clue is then revealed and each player writes down their answer (in the form of a question).");
                    Console.WriteLine("The players that answer correctly will add the points they wagered to their current amount of points.");
                    Console.WriteLine("The players that answer incorrectly will lose the points they wagered.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // daily doubles
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Daily Doubles: \n");
                    Console.ResetColor();


                    Console.WriteLine("In the Jeopardy and Double Jeopardy rounds, the players can run into Daily Doubles.\n");
                    Console.WriteLine("At the beginning of the round, one of the clues are chosen to be a Daily Double.\n");
                    Console.WriteLine("This means that, instead of all the players buzzing in to answer, the player who chose the clue gets to choose an amount of points to wager before the clue is revealed.");
                    Console.WriteLine("Once the clue is revealed only that player gets to answer (with a question).\n");
                    Console.WriteLine("If they answer correctly, they earn the amount of points they wagered.");
                    Console.WriteLine("If they answer incorrectly, they lose the amount of points they wagered.\n");
                    Console.WriteLine("In Double Jeopardy, two of the clues will be a Daily Double.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to go back to the menu.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                }

                // credits
                else if (menuChoice == "3")
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Credits: \n");
                    Console.ResetColor();

                    Console.WriteLine("\tProgrammed by Inge DTS.");
                    Console.WriteLine("\tCategories, clues and answers written by Louise and Chris.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to go back to the menu.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                }

                // exit game
                else if (menuChoice == "4")
                {
                    break;
                }

                // invalid input
                else
                {
                    InvalidInput();
                }
            }

            // methods vvv

            /// <summary>
            /// The ManageRounds method sets up the game and manages which round is being played.
            /// </summary>
            static void ManageRounds()
            {
                Console.Clear();
                Console.ResetColor();

                // player information
                string[] playerName = { "Player 1", "Player 2", "Player 3" };
                ConsoleColor[] playerColor = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Blue };
                ConsoleKey[] playerBuzzer = { ConsoleKey.A, ConsoleKey.L, ConsoleKey.G };
                bool[] playerAllowed = { false, false, false }; // to allow/disallow players from participating in BuzzIn or FinalJeopardy
                int[] playerPoints = { 0, 0, 0 };

                // game setup
                // ask for the amount of players until getting a valid input
                int amountOfPlayers = 0;

                while (amountOfPlayers == 0)
                {
                    Console.WriteLine("How many will be playing? (1-3)");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nInput a number from 1-3 and press enter.");
                    Console.ResetColor();

                    if (int.TryParse(Console.ReadLine(), out int amount))
                    {
                        if (amount > 0 && amount < 4) amountOfPlayers = amount;
                    }
                }

                // ask players for their name
                EnterName(ref playerName[0], playerBuzzer[0], playerColor[0]);
                playerAllowed[0] = true;

                if (amountOfPlayers > 1)
                {
                    EnterName(ref playerName[1], playerBuzzer[1], playerColor[1]);
                    playerAllowed[1] = true;

                    if (amountOfPlayers > 2)
                    {
                        EnterName(ref playerName[2], playerBuzzer[2], playerColor[2]);
                        playerAllowed[2] = true;
                    }
                }

                // quiz content for the Jeopardy round
                string[] jeopardyCategories = { "Music", "Science", "Movies", "Sports", "History", "Food" };
                string[,] jeopardyClues =
                {
                {"She’s been called “The Queen of Pop” and had hits like “Like A Prayer” and “Vogue”.", "He composed music for movies like The Pirates of the Carribbeans, The Lion King, Gladiator and Interstellar.", "She played Glinda in Wicked opposite Cynthia Erivo as Elphaba.", "Her full name is Stefani Joanne Angelina Germanotta.", "Beyoncé surprise-released this album in 2016." },
                {"He was credited with the invention of the theory of relativity.", "It makes your voice highpitched when you inhale it, and it makes balloons float.", "Fundamental building blocks of matter.", "Buzz Aldrin and Neil Armstrong landed here in 1969.", "3,14159" },
                {"In this Tim Burton movie five kids visit a factory of a peculiar chocolatier.", "This movie is famous for “the lift” and the naughty dancing.", "In this movie Olivia Newton-John gets a makeover in the end to impress John Travolta.", "John Travolta plays a woman and sings in this movie from 2007.", "Regina George is hit by a bus in this movie from 2004." },
                {"This state has Fenway Park,  the oldest baseball stadium in the US.", "The 2024 Summer Olympics were  held in this city.", "This team has the most Super Bowl wins.", "This football player just got married to Taylor Swift.", "This soccer player married a Spice Girl." },
                {"She was quoted saying: “Let them eat cake”.", "A prominent leader of the civil rights movement from 1955 until his assassination in 1968.", "His middle name was Fitzgerrald and he was the president of the United States from 1961 until his assination in 1963.", "He was stabbed at least 23 times.", "This king was married six times and ordered the beheadings of two of his wives." },
                {"Fries and chips are made out of this vegetable.", "Samwise Gamgee from Lord of the Rings is very passionate about this vegetable.", "Sometimes used to brew alcoholic spirits such as vodka, poitín, akvavit, and brännvin.", "There are at least 16 museums dedicated to this vegetable.", "Mads Mikkelsen tries to plant and harvest this in the movie The Promised Land from 2023." } };
                string[,] jeopardyAnswers =
                {
                {"Who is Madonna?", "Who is Hans Zimmer?", "Who is Ariana Grande?", "Who is Lady Gaga?", "What is Lemonade?" },
                {"Who is Albert Einstein?", "What is helium?", "What are atoms?", "What is the moon?", "What is pi?" },
                {"What is Charlie and the Chocolate Factory?", "What is Dirty Dancing?", "What is Grease?", "What is Hairspray?", "What is Mean Girls?" },
                {"What is Massachusetts?", "What is Paris?", "Who are The New England Patriots?", "Who is Travis Kelce?", "Who is David Beckham?" },
                {"Who is Marie Antoinette?", "Who is Martin Luther King Jr.?", "Who is John F. Kennedy?", "Who is Julius Caesar?", "Who is King Henry VIII?" },
                {"What are potatoes?", "What are potatoes?", "What are potatoes?", "What are potatoes?", "What are potatoes?" } };

                // start the Jeopardy round
                PlayJeopardy(playerName, playerColor, playerBuzzer, ref playerAllowed, ref playerPoints, amountOfPlayers, 200, jeopardyCategories, ref jeopardyClues, jeopardyAnswers);

                // after the Jeopardy round:
                Console.Clear();
                Console.ResetColor();

                // ask if they want to continue with the double jeopardy round
                Console.WriteLine("Do you want to continue and play the Double Jeopardy round?\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If no: press tab.");
                Console.WriteLine("If yes: press any other key.");
                Console.ResetColor();

                if (Console.ReadKey().Key != ConsoleKey.Tab)
                {
                    // quiz content for the Double Jeopardy round
                    string[] doubleJeopardyCategories = { "Lyrics", "Politics", "Television", "Literature", "Famous People", "Christmas" };
                    string[,] doubleJeopardyClues =
                    {
                {"They sang: “I am the walrus”.", "She sang: “I’m a genie in a bottle, you gotta rub me the right way”.", "He sang: “You can tell everybody that this is your song”.", "She sang: “And I will always love you”.", "He sang: “I thought you’d always be mine”." },
                {"Vice president of The United States.", "First black president of The United States of America.", "He was the US president during the 9/11 attacks.", "She was the longest-serving British prime minister of the 20th century and the first woman to hold the office.", "Gated street in Westminster in London that houses the official residences and offices of the Prime Minister of the United Kingdom." },
                {"This guy is yellow and his best friend is a starfish.", "She and her friends, Freddy and Sam make a webshow named after her.", "In this show Jesse and Walter White cook meth.", "Lena Dunham wrote and starred in this show from the 2010s.", "Friend to Ross, Monica, Rachel, Joey and Phoebe." },
                {"His first publication was “Carrie” and he has since published more than 60 titles.", "She published Frankenstein when she was 20 years old.", "She invented the character Mr. Darcy.", "Transylvanian count written about by Bram Stoker.", "In this book 17 year old Bella falls in love with Edward and finds out that he’s a vampire." },
                {"Her Spice Girl name before she got married to David Beckham.", "She is a sister to Bella and  she has a daughter with Zayn Malik.", "She loves auto-tune and she plays Meryl Streep's mother in the movie Mama Mia! Here We Go Again.", "He was in The Social Network and Call Me By Your Name but he also got famous for sexual assault and cannibalism.", "He has been portrayed on screen by Jesse Eisenberg and now Jeremy Strong. He likes blue." },
                {"This song has been played on the radio every Christmas since 1994 and is sung by Mariah Carey.", "“Last Christmas” is sung by this English duo.", "If not a star, this is typically at the top of the christmas tree.", "These animals pull Santa’s sleigh.", "You might get this in your sock if you have been naughty." } };
                    string[,] doubleJeopardyAnswers =
                    {
                {"Who are The Beatles?", "Who is Christina Aguilera?", "Who is Elton John?", "Who is Whitney Houston?", "Who is Justin Bieber?" },
                {"Who is JD Vance?", "Who is Barack Obama?", "Who is George W Bush?", "Who is Margaret Thatcher?", "What is Downing Street?" },
                {"Who is Spongebob Squarepants?", "Who is Carly from iCarly?", "What is Breaking Bad?", "What is Girls?", "Who is Chandler Bing?" },
                {"Who is Stephen King?", "Who is Mary Shelley?", "Who is Jane Austen?", "Who is Dracula?", "What is Twilight?" },
                {"Who is Posh Spice?", "Who is Gigi Hadid?", "Who is Cher?", "Who is Armie Hammer?", "Who is Mark Zuckerberg?" },
                {"What is All I Want for Christmas is You?", "Who is Wham!?", "What is an angel?", "What are reindeers?", "What is coal?" } };

                    // start the Double Jeopardy round
                    PlayJeopardy(playerName, playerColor, playerBuzzer, ref playerAllowed, ref playerPoints, amountOfPlayers, 400, doubleJeopardyCategories, ref doubleJeopardyClues, doubleJeopardyAnswers);
                }

                // after the Double Jeopardy round:
                Console.Clear();
                Console.ResetColor();

                // ask if they want to continue with final jeopardy round
                Console.WriteLine("Do you want to continue with the Final Jeopardy round?\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If no: press tab.");
                Console.WriteLine("If yes: press any other key.");
                Console.ResetColor();

                if (Console.ReadKey().Key != ConsoleKey.Tab)
                {
                    // start Final Jeopardy round
                    PlayFinalJeopardy(playerName, playerColor, ref playerAllowed, ref playerPoints);
                }

                // after the Final Jeopardy round
                Console.Clear();
                Console.ResetColor();

                // display who won the game
                ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);

                int winner = MostPoints(playerPoints);

                if (winner == -1)
                {
                    Console.WriteLine("\n\nCongratulation! It's a tie!");
                }
                else
                {
                    Console.Write("\nCongratulations, ");
                    Console.ForegroundColor = playerColor[winner];
                    Console.Write(playerName[winner]);
                    Console.ResetColor();
                    Console.WriteLine("! You won!");
                }

                Console.WriteLine("\n\n\nThank you for playing Jeopardy!");

                // go back to jeopardy menu
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to ´return to the menu.");
                Console.ResetColor();
                Console.ReadKey();

                Console.Clear();
            }

            /// <summary>
            /// The PlayJeopardy method contains the Jeopardy and Double Jeopardy game loop.
            /// </summary>
            /// <param name="playerName">
            /// Array of the players' chosen names.
            /// </param>
            /// <param name="playerColor">
            /// Array of the players' colors.
            /// </param>
            /// <param name="playerBuzzer">
            /// Array of the players' assigned buzzer-keys.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether each player is allowed to participate in BuzzIn.
            /// </param>
            /// <param name="playerPoints">
            /// Array of the players' points.
            /// </param>
            /// <param name="amountOfPlayers">
            /// The amount of players in the game.
            /// </param>
            /// <param name="lowestCluePointAmount">
            /// The lowest amount of points a clue can give in the round (200 for Jeopardy/400 for Double Jeopardy)
            /// </param>
            /// <param name="categories">
            /// Array of categories for the round.
            /// </param>
            /// <param name="clues">
            /// Array of clues for the round.
            /// </param>
            /// <param name="answers">
            /// Array of correct answers for the round.
            /// </param>
            static void PlayJeopardy(string[] playerName, ConsoleColor[] playerColor, ConsoleKey[] playerBuzzer, ref bool[] playerAllowed, ref int[] playerPoints, int amountOfPlayers, int lowestCluePointAmount, string[] categories, ref string[,] clues, string[,] answers)
            {
                Console.Clear();
                Console.ResetColor();

                // choose daily doubles clue
                Random random = new Random();

                int dailyDoubleCategory = random.Next(0, 6);
                int dailyDoubleClue = random.Next(0, 5);

                int doubleDailyDoubleCategory = -1;
                int doubleDailyDoubleClue = -1;

                // if it's the Double Jeopardy round choose another daily double
                if (lowestCluePointAmount == 400)
                {
                    while (true)
                    {
                        doubleDailyDoubleCategory = random.Next(0, 6);
                        doubleDailyDoubleClue = random.Next(0, 5);

                        if (doubleDailyDoubleCategory == dailyDoubleCategory && doubleDailyDoubleClue == dailyDoubleClue)
                        {
                            continue;
                        }
                        else break;
                    }
                }

                // decide who goes first
                Console.WriteLine("Use your buzzer to choose who goes first!\n\n");

                int clueChooser;

                if (amountOfPlayers > 1)
                {
                    if (lowestCluePointAmount == 200) clueChooser = BuzzIn(playerName, playerBuzzer, playerAllowed, true);
                    else if (lowestCluePointAmount == 400)
                    {
                        // find the player with the least points,
                        // if there's a tie it will be the last player with that amount of points on the list
                        int leastPoints = playerPoints[0];
                        clueChooser = 0;

                        for (int i = 0; i < amountOfPlayers; i++)
                        {
                            if (playerPoints[i] <= leastPoints)
                            {
                                leastPoints = playerPoints[i];
                                clueChooser = i;
                            }
                        }
                    }
                    else clueChooser = 0;
                }
                else clueChooser = 0;

                // Jeopardy/Double Jeopardy game loop
                while (true)
                {
                    Console.Clear();
                    Console.ResetColor();

                    // check if there are any clues left - if not end the round
                    bool cluesLeft = true;

                    foreach (string clue in clues)
                    {
                        if (clue != "")
                        {
                            cluesLeft = true;

                            break;
                        }
                        else cluesLeft = false;
                    }

                    if (!cluesLeft) break;

                    // allow all players to buzz in for the upcoming clue
                    playerAllowed[0] = true;

                    if (amountOfPlayers > 1)
                    {
                        playerAllowed[1] = true;

                        if (amountOfPlayers > 2)
                        {
                            playerAllowed[2] = true;
                        }
                    }

                    // write out points
                    ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);

                    Console.ResetColor();
                    Console.WriteLine("\n\n");

                    // write out categories
                    for (int i = 0; i < categories.Length; i++)
                    {
                        Console.Write(categories[i] + "       ");
                    }

                    Console.WriteLine("\n");

                    // write out the rest of the board - gray out clues that have been picked before
                    for (int col = 0; col < clues.GetLength(1); col++)
                    {
                        for (int row = 0; row < clues.GetLength(0); row++)
                        {
                            if (clues[row, col] == "") Console.ForegroundColor = ConsoleColor.DarkGray;

                            int cluePointAmount = (col + 1) * lowestCluePointAmount;

                            if (lowestCluePointAmount == 200)
                            {
                                Console.Write(" " + cluePointAmount + (cluePointAmount < 1000 ? "         " : "        "));
                            }
                            if (lowestCluePointAmount == 400)
                            {
                                Console.Write(" " + cluePointAmount + (cluePointAmount < 1000 ? "              " : "             "));
                            }
                            Console.ResetColor();
                        }
                        Console.WriteLine("\n");
                    }

                    Console.WriteLine("\n");

                    // clueChooser chooses category
                    Console.ForegroundColor = playerColor[clueChooser];
                    Console.Write(playerName[clueChooser]);
                    Console.ResetColor();
                    Console.WriteLine(", please choose a category.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Input the category name or number and press enter.");
                    Console.ResetColor();

                    string chosenCategory = Console.ReadLine() ?? "";

                    int categoryNumber = -1;

                    // check if categories match name
                    for (int i = 0; i < categories.Length; i++)
                    {
                        if (chosenCategory.ToUpper().Trim() == categories[i].ToUpper())
                        {
                            categoryNumber = i;
                            break;
                        }
                    }

                    // check if categories match number
                    if (categoryNumber == -1 && int.TryParse(chosenCategory, out int result))
                    {
                        if (!(result < 1) && !(result > 6))
                        {
                            categoryNumber = result - 1;
                        }
                    }

                    // if categories did not match, make player try again
                    if (categoryNumber == -1)
                    {
                        InvalidInput("category");
                        continue;
                    }

                    // clueChooser chooses clue
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nChosen category: " + categories[categoryNumber]);
                    Console.ResetColor();

                    Console.ForegroundColor = playerColor[clueChooser];
                    Console.Write(playerName[clueChooser]);
                    Console.ResetColor();
                    Console.WriteLine(", please choose the clue you want.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Input the point-amount of the clue you want and press enter.");
                    Console.ResetColor();

                    string chosenClue = Console.ReadLine() ?? "";

                    // check if input is valid
                    if (int.TryParse(chosenClue, out int clueNumber))
                    {
                        if (lowestCluePointAmount == 200)
                        {
                            switch (clueNumber)
                            {
                                case 200:
                                    clueNumber = 0;
                                    break;
                                case 400:
                                    clueNumber = 1;
                                    break;
                                case 600:
                                    clueNumber = 2;
                                    break;
                                case 800:
                                    clueNumber = 3;
                                    break;
                                case 1000:
                                    clueNumber = 4;
                                    break;
                                default:
                                    InvalidInput("clue");
                                    continue;
                            }
                        }
                        else if (lowestCluePointAmount == 400)
                        {
                            switch (clueNumber)
                            {
                                case 400:
                                    clueNumber = 0;
                                    break;
                                case 800:
                                    clueNumber = 1;
                                    break;
                                case 1200:
                                    clueNumber = 2;
                                    break;
                                case 1600:
                                    clueNumber = 3;
                                    break;
                                case 2000:
                                    clueNumber = 4;
                                    break;
                                default:
                                    InvalidInput("clue");
                                    continue;
                            }
                        }
                    }
                    else
                    {
                        InvalidInput("clue");
                        continue;
                    }

                    // check if clue has already been picked
                    if (clues[categoryNumber, clueNumber] == "")
                    {
                        InvalidInput("clue");
                        continue;
                    }

                    Console.Clear();

                    // answer "screen"
                    while (true)
                    {
                        int answeringPlayer;

                        // Daily Double
                        if (categoryNumber == dailyDoubleCategory && clueNumber == dailyDoubleClue || lowestCluePointAmount == 400 && categoryNumber == doubleDailyDoubleCategory && clueNumber == doubleDailyDoubleClue)
                        {
                            answeringPlayer = clueChooser;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Daily Double!\n");
                            Console.ResetColor();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n\n");
                            Console.ResetColor();

                            // make wager
                            Console.ForegroundColor = playerColor[answeringPlayer];
                            Console.Write(playerName[answeringPlayer]);
                            Console.ResetColor();
                            Console.WriteLine(", you get to make a wager:\n");

                            int dailyDoubleWager = WagePoints(playerName[answeringPlayer], playerColor[answeringPlayer], playerPoints[answeringPlayer], clues.GetLength(1) * lowestCluePointAmount);

                            Console.Clear();
                            Console.ResetColor();

                            // display wager and category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("You have wagered " + dailyDoubleWager + " points on the category " + categories[categoryNumber] + " for " + chosenClue + ".\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("Your clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber]);
                        }
                        else
                        {
                            // check if there are more than one person left to answer
                            int buzzersLeft = BuzzersLeft(playerAllowed);

                            if (buzzersLeft < 1)
                            {
                                Console.Clear();

                                // display category
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                                Console.ResetColor();

                                // display clue
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("The clue was: ");
                                Console.ResetColor();
                                Console.WriteLine(clues[categoryNumber, clueNumber]);

                                // display answer
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("\nThe correct answer is: ");
                                Console.ResetColor();
                                Console.WriteLine(answers[categoryNumber, clueNumber]);

                                // go back to board
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("\n\nPress any key to continue.");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            }

                            Console.Clear();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber] + "\n");

                            // pick which player who's gonna answer the question
                            if (buzzersLeft >= 2)
                            {
                                answeringPlayer = BuzzIn(playerName, playerBuzzer, playerAllowed);
                            }
                            else
                            {
                                answeringPlayer = LastBuzzer(playerAllowed);

                                // don't force player to answer (unless they are playing alone)
                                if (amountOfPlayers > 1)
                                {
                                    Console.ForegroundColor = playerColor[answeringPlayer];
                                    Console.Write(playerName[answeringPlayer]);
                                    Console.ResetColor();
                                    Console.WriteLine(", do you want to answer?");

                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("Press tab if no, press any other key if yes.");
                                    Console.ResetColor();

                                    if (Console.ReadKey().Key == ConsoleKey.Tab) break;
                                }
                            }
                        }

                        // go back to board if no player has an answer
                        if (answeringPlayer == -1) break;

                        // ask for the player to answer
                        string answerInput = WriteAnswer(playerName[answeringPlayer], playerColor[answeringPlayer]);

                        // check if the answer is correct
                        bool correctAnswer = CorrectAnswer(answerInput, answers[categoryNumber, clueNumber]);

                        // inform player if their answer is correct
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Your answer was: " + answerInput + ".\n");
                        Console.ResetColor();

                        Console.ForegroundColor = correctAnswer ? ConsoleColor.Green : ConsoleColor.Magenta;
                        Console.WriteLine("Your answer is " + (correctAnswer ? "CORRECT" : "WRONG") + "!");
                        Console.ResetColor();

                        // add/subtract points
                        if (correctAnswer)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("+ " + chosenClue);
                            Console.ResetColor();

                            playerPoints[answeringPlayer] += int.Parse(chosenClue);

                            clueChooser = answeringPlayer;

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("- " + chosenClue);
                            Console.ResetColor();

                            playerPoints[answeringPlayer] -= int.Parse(chosenClue);

                            playerAllowed[answeringPlayer] = false;

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();
                        }

                        // if it was a daily double clue or if there's only one player: go back to the board without repeating anything
                        if (categoryNumber == dailyDoubleCategory && clueNumber == dailyDoubleClue || lowestCluePointAmount == 400 && categoryNumber == doubleDailyDoubleCategory && clueNumber == doubleDailyDoubleClue)
                        {
                            Console.Clear();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The clue was: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber]);

                            // display answer
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nThe correct answer is: ");
                            Console.ResetColor();
                            Console.WriteLine(answers[categoryNumber, clueNumber]);

                            // go back to board
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();
                            Console.ReadKey();

                            break;
                        }
                    }

                    // set the clue to be an empty string (grayed out and invalid choice)
                    clues[categoryNumber, clueNumber] = "";

                    Console.Clear();
                    Console.ResetColor();

                    // continue/end game
                    Console.WriteLine("If you want to end the round, press tab.");
                    Console.WriteLine("Press any other key to continue.");

                    if (Console.ReadKey().Key == ConsoleKey.Tab)
                    {
                        break;
                    }
                }
            }

            /// <summary>
            /// The PlayFinalJeopardy method contains the Final Jeopardy round.
            /// </summary>
            /// <param name="playerName">
            /// Array of the players' chosen names.
            /// </param>
            /// <param name="playerColor">
            /// Array of the players' colors.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether each player is allowed to participate in BuzzIn.
            /// </param>
            /// <param name="playerPoints">
            /// Array of the players' points.
            /// </param>
            static void PlayFinalJeopardy(string[] playerName, ConsoleColor[] playerColor, ref bool[] playerAllowed, ref int[] playerPoints)
            {
                Console.Clear();

                // check if players have more than 0 points - exclude the ones that don't
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerPoints[i] > 0)
                    {
                        playerAllowed[i] = true;
                    }
                    else playerAllowed[i] = false;
                }

                // display which players will play in the Final Jeopardy round
                Console.WriteLine("The following players qualify for the Final Jeopardy round: \n");

                int playersLeft = 0;

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        playersLeft += 1;
                        Console.ForegroundColor = playerColor[i];
                        Console.WriteLine("\t" + playerName[i]);
                        Console.ResetColor();
                    }
                }

                if (playersLeft < 1)
                {
                    Console.WriteLine("\tNo player qualifies.");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to continue.");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();

                if (playersLeft > 0)
                {
                    // content for Final Jeopardy round
                    string[,] clues = { { "Geography", "Video games", "Language" }, { "Island country east of Australia where The Lord of the Rings were filmed.", "He’s an Italian plumber.", "The Inuit in Greenland are believed to have over 40 words for this weather phenomenon." }, { "What is New Zealand?", "Who is Super Mario?", "What is snow?" } };

                    // choose a random final clue
                    Random random = new Random();
                    int finalClue = random.Next(0, clues.GetLength(1));

                    // let players wager points
                    int[] playerWagers = { 0, 0, 0 };

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.WriteLine("The final category is " + clues[0, finalClue] + ".\n\n");

                            Console.Write("Everyone, except ");
                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(", should look away now.\n");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(playerName[i] + ", press any key when you are ready to make a wager.");
                            Console.ResetColor();
                            Console.ReadKey();

                            Console.Clear();
                            playerWagers[i] = WagePoints(playerName[i], playerColor[i], playerPoints[i]);

                            Console.Clear();
                            Console.WriteLine("You have wagered " + playerWagers[i] + " points.");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            Console.Clear();
                        }
                    }

                    // let players answer
                    string[] playerAnswers = { "", "", "" };

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            // display category and clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The final category is " + clues[0, finalClue] + ".\n");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The final clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[1, finalClue]);

                            // all players write their answer
                            Console.Write("\n\nEveryone, except ");
                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(", should look away now.\n");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(playerName[i] + ", press any key when you are ready to write your answer.");
                            Console.ResetColor();
                            Console.ReadKey();

                            playerAnswers[i] = WriteAnswer(playerName[i], playerColor[i]);

                            Console.Clear();
                            Console.WriteLine("You have answered " + playerAnswers[i]);

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            Console.Clear();
                        }
                    }

                    // all the answers and whether they are correct are revealed one by one
                    Console.WriteLine("Now the answers will be revealed: \n");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to reveal the next player's answer.\n\n");
                    Console.ResetColor();

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.ReadKey();

                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(": " + playerAnswers[i] + ".");

                            Console.ReadKey();

                            if (CorrectAnswer(playerAnswers[i], clues[2, finalClue]))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("   CORRECT");
                                Console.ResetColor();

                                playerPoints[i] += playerWagers[i];
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("   INCORRECT");
                                Console.ResetColor();

                                playerPoints[i] -= playerWagers[i];
                            }
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // reveal how many points each player wagered on the final clue
                    Console.WriteLine("Points wagered: \n");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to reveal the next player's wager.\n\n");
                    Console.ResetColor();

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.ReadKey();

                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.WriteLine(" wagered " + playerWagers[i] + " of their points.");
                        }
                    }
                }

                // end final jeopardy
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to end the game and see the results.");
                Console.ResetColor();

                Console.ReadKey();
            }

            /// <summary>
            /// The InvalidInput method informs the player that their input is invalid.
            /// </summary>
            /// <param name="input">
            /// What kind of input that is invalid. Default is "input".
            /// </param>
            static void InvalidInput(string input = "input")
            {
                Console.WriteLine("\nInvalid " + input + ".");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n\nPress any key to continue.");
                Console.ResetColor();

                Console.ReadKey();
            }

            /// <summary>
            /// The EnterName method informs the players' of their buzzer-key and asks them to write their name.
            /// </summary>
            /// <param name="player">A specific player's name/alias.</param>
            /// <param name="buzzer">A specific player's assigned buzzer-key.</param>
            /// <param name="color">A specific player's color.</param>
            static void EnterName(ref string player, ConsoleKey buzzer, ConsoleColor color)
            {
                while (true)
                {
                    Console.Clear();

                    // display assigned buzzer
                    Console.ForegroundColor = color;
                    Console.Write(player);
                    Console.ResetColor();
                    Console.Write(" will have the ");
                    Console.ForegroundColor = color;
                    Console.Write(buzzer + "-key");
                    Console.ResetColor();
                    Console.WriteLine(" as their buzzer.");

                    // ask for the player's name
                    Console.WriteLine("\nPlease write the name of " + player + ".");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Write your name and press enter.");

                    string name = Console.ReadLine() ?? "";

                    if (name != "")
                    {
                        player = name;
                        break;
                    }
                    else InvalidInput();
                }
            }

            /// <summary>
            /// The BuzzersLeft method checks how many players are allowed to BuzzIn.
            /// </summary>
            /// <returns>
            /// Returns an integer of how many players are allowed to BuzzIn.
            /// </returns>
            /// <param name="playerAllowed">
            /// An array of booleans determining which players are allowed to BuzzIn.
            /// </param>
            static int BuzzersLeft(bool[] playerAllowed)
            {
                int buzzers = 0;

                foreach (bool buzzerAllowed in playerAllowed)
                {
                    if (buzzerAllowed)
                    {
                        buzzers += 1;
                    }
                }

                return buzzers;
            }

            /// <summary>
            /// The LastBuzzer method checks which player is the last one allowed to BuzzIn.
            /// </summary>
            static int LastBuzzer(bool[] playerAllowed)
            {
                int lastBuzzer = -1;

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        lastBuzzer = i;
                        return lastBuzzer;
                    }
                }

                return lastBuzzer;
            }

            /// <summary>
            /// The BuzzIn method asks the players to press their buzzer-key and returns the winner.
            /// </summary>
            /// <returns>
            /// Returns the fastest player to press their buzzer-key.
            /// </returns>
            /// <param name="playerName">
            /// Array of the players' names.
            /// </param>
            /// <param name="playerBuzzer">
            /// Array of the players' buzzer-keys.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether the players' are allowed to BuzzIn.
            /// </param>
            /// <param name="clueChoice">
            /// Whether the players' are buzzing in to answer a clue or not. Default is false.
            /// </param>
            static int BuzzIn(string[] playerName, ConsoleKey[] playerBuzzer, bool[] playerAllowed, bool clueChoice = false)
            {
                int buzzWinner;

                Console.WriteLine("\nGet ready to buzz in!");

                // remind players of their buzzer-keys
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nRemember your buzz-keys: \n");
                Console.ResetColor();

                for (int i = 0; i < playerName.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        Console.Write(playerName[i]);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(", your buzzer-key is ");
                        Console.ResetColor();
                        Console.WriteLine(playerBuzzer[i]);
                    }
                }

                Console.WriteLine("\n\nYou can buzz in after the beep.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If you don't hear a beep, you can buzz in when 'BUZZ IN NOW' is written in green.");


                Console.WriteLine("\n\nWhen you are all ready to buzz in, press any key to continue.");
                Console.ResetColor();

                Console.ReadKey();

                // let players buzz in
                Console.Beep(700, 1000);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("BUZZ IN NOW");
                Console.ResetColor();

                // let players skip the question if none of them have an answer
                if (!clueChoice)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("If none of you know the answer, you can skip answering by pressing tab.");
                    Console.ResetColor();
                }

                // find out who buzzed in the fastest
                while (true)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey();

                    if (pressedKey.Key == playerBuzzer[0] && playerAllowed[0])
                    {
                        buzzWinner = 0;
                        break;
                    }
                    else if (pressedKey.Key == playerBuzzer[1] && playerAllowed[1])
                    {
                        buzzWinner = 1;
                        break;
                    }
                    else if (pressedKey.Key == playerBuzzer[2] && playerAllowed[2])
                    {
                        buzzWinner = 2;
                        break;
                    }
                    else if (pressedKey.Key == ConsoleKey.Tab && !clueChoice)
                    {
                        buzzWinner = -1;
                        break;
                    }
                    else continue;

                }

                return buzzWinner;

            }

            /// <summary>
            /// The WriteAnswer method asks the player to write their answer to a clue and outputs their answer.
            /// </summary>
            /// <returns>
            /// Returns the player's answer to the clue.
            /// </returns>
            /// <param name="playerName">
            /// The specific player's name.
            /// </param>
            /// <param name="playerColor">
            /// The specific player's color.
            /// </param>
            static string WriteAnswer(string playerName, ConsoleColor playerColor)
            {
                Console.WriteLine("\n");
                Console.ForegroundColor = playerColor;
                Console.Write(playerName);
                Console.ResetColor();
                Console.WriteLine(", please write your answer and press enter.");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Remember to form your answer as a question (what is/what are/who is/who are/etc).");
                Console.ResetColor();

                return Console.ReadLine() ?? "";
            }

            /// <summary>
            /// The CorrectAnswer method checks whether the player's answer is correct or incorrect.
            /// </summary>
            /// <returns>
            /// Returns true if the player's answer is correct and false if incorrect.
            /// </returns>
            /// <param name="answerInput">
            /// The player's answer.
            /// </param>
            /// <param name="correctAnswer">
            /// The correct answer.
            /// </param>
            static bool CorrectAnswer(string answerInput, string correctAnswer)
            {
                if (answerInput.ToUpper().Trim() == correctAnswer.ToUpper())
                {
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// The ShowPoints method displays how many points each player has earned.
            /// </summary>
            /// <param name="playerColor">
            /// An array of the players' colors.
            /// </param>
            /// <param name="playerName">
            /// An array of the players' names.
            /// </param>
            /// <param name="playerPoints">
            /// An array of the players' points.
            /// </param>
            /// <param name="amountOfPlayers">
            /// The amount of players in the game.
            /// </param>
            static void ShowPoints(ConsoleColor[] playerColor, string[] playerName, int[] playerPoints, int amountOfPlayers)
            {
                Console.ForegroundColor = playerColor[0];
                Console.Write("\t\t" + playerName[0]);
                Console.ResetColor();
                Console.Write(": " + playerPoints[0] + " pts");

                if (amountOfPlayers > 1)
                {
                    Console.ForegroundColor = playerColor[1];
                    Console.Write("\t" + playerName[1]);
                    Console.ResetColor();
                    Console.Write(": " + playerPoints[1] + " pts");
                    if (amountOfPlayers > 2)
                    {
                        Console.ForegroundColor = playerColor[2];
                        Console.Write("\t" + playerName[2]);
                        Console.ResetColor();
                        Console.Write(": " + playerPoints[2] + " pts");
                    }
                }
            }

            /// <summary>
            /// The WagePoints method asks the player to wager an amount of points.
            /// </summary>
            /// <returns>
            /// Returns the player's wager.
            /// </returns>
            /// <param name="playerName">
            /// The specific player's name.
            /// </param>
            /// <param name="playerColor">
            /// The specific player's color.
            /// </param>
            /// <param name="playerPoints">
            /// The specific player's amount of points.
            /// </param>
            /// <param name="mostPointsOnBoard">
            /// The highest amount of points a clue can give on either the Jeopardy or Double Jeopardy board. The default is 0 for use in Final Jeopardy.
            /// </param>
            static int WagePoints(string playerName, ConsoleColor playerColor, int playerPoints, int mostPointsOnBoard = 0)
            {
                Console.WriteLine("\n\n");

                // inform player of the amount of points they have and how much they can wager
                Console.ForegroundColor = playerColor;
                Console.Write(playerName);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(", you have earned " + playerPoints + " points at this point.\n");
                Console.ResetColor();

                int maxWager = playerPoints > mostPointsOnBoard ? playerPoints : mostPointsOnBoard;
                Console.WriteLine("You can wager between 1 and " + maxWager + ".");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If your answer is correct, you will add the amount of points you wagered to your current score. \nIf your answer is incorrect, you will lose the amount of points you wagered. \n\n");
                Console.ResetColor();

                // let the player wage their points
                while (true)
                {
                    Console.WriteLine("Please input the amount of points you would like to wager and press enter.");

                    if (int.TryParse(Console.ReadLine(), out int pointsWagered) && pointsWagered > 0 && pointsWagered <= maxWager)
                    {
                        return pointsWagered;
                    }
                    else
                    {
                        InvalidInput("wager");
                        continue;
                    }
                }
            }

            /// <summary>
            /// The MostPoints method checks which player has the most points.
            /// </summary>
            /// <returns>
            /// Returns the player with the most points or -1 if there is a tie.
            /// </returns>
            /// <param name="playerPoints">
            /// An array of the players' points.
            /// </param>
            static int MostPoints(int[] playerPoints)
            {
                int playerWithMostPoints = 0;

                // go through the players' points and set the last player with most points as the player with most points
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerWithMostPoints == i || playerPoints[playerWithMostPoints] < playerPoints[i])
                    {
                        playerWithMostPoints = i;
                    }
                }

                // check if any other player has the same amount of points
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerPoints[playerWithMostPoints] == playerPoints[i] && playerWithMostPoints != i)
                    {
                        playerWithMostPoints = -1;
                        break;
                    }
                }

                return playerWithMostPoints;
            }
        }
    }
}
