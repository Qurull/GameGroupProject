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
                        return;
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
                    PrintMasterMindMenu((2, 1));
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
    }
}
