using System.Diagnostics;
using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        // In order for program to look good, font must be Consolas size 16, bold.
        static (string name, int gridSizeX, int gridSizeY, int bombs)[] difficulties = new (string, int, int, int)[]
        {
            ("Easy", 9, 9, 10),
            ("Medium", 16, 16, 40),
            ("Hard", 30, 16, 99),
            ("Extreme", 40, 30, 400)
        };
        static ConsoleColor selectorColor = ConsoleColor.White;
        static ConsoleColor headlineColor = ConsoleColor.DarkRed;
        static bool gridGenerated = false;
        static bool playerDied = false;
        static char bombChar = '*';
        static char[] possibleFlagChars = {'F', 'P', 'X', '!', '?', '¤', bombChar, '#', 'O'};
        static int selectedFlagChar = 0;
        static int gridSizeX;
        static int gridSizeY;
        static int bombs;
        static int bombsMinusFlags;
        static char[,] grid = new char[gridSizeX, gridSizeY];
        static bool[,] isRevealed = new bool[gridSizeX, gridSizeY];
        static bool[,] flagPlaced = new bool[gridSizeX, gridSizeY];
        static Random random = new Random();
        static (int x, int y) cursor = (0,0);
        static bool timerIsRunning = false;
        static int timeSinceGameStart = 0;
        static string currentSmiley = ":)";
        static ManualResetEvent mre1 = new ManualResetEvent(true);
        static ManualResetEvent mre2 = new ManualResetEvent(true);
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            /*
            Console.Write("Grid size x: ");
            while (!int.TryParse(Console.ReadLine(), out gridSizeX)) ;
            Console.Write("Grid size y: ");
            while (!int.TryParse(Console.ReadLine(), out gridSizeY)) ;
            */
            while (MinesweeperMenu())
            {
                Minesweeper();
            }
        }
        static bool MinesweeperMenu() // Returns true if the player starts the game, false if the player quits.
        {
            // All of this code is so dogshit, I just wanna get this over with, ignore it
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

                // There is a disgusting amount of temporary stupid useless variables and hardcoded bullshit in this method, I want to throw up
                // RAAAAAAGH I FUCKING HATE THIS SHIT THIS IS SO ASS JUST WORK FOR FUCKS SAKE :sob:
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

                    if(key.Key != ConsoleKey.Escape)
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
                            for(int i = 0; i < difficulties.Length * 2 + 2; i++)
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
                                else if((keyInt > 47 && keyInt < 58) || (keyInt > 95 && keyInt < 106))
                                {
                                    // Pressed a number between 0 and 9.
                                    customValues[selectedIndex - newZero] += key.KeyChar;
                                }
                                else if(key.Key == ConsoleKey.Backspace && customValues[selectedIndex - newZero].Length > 0)
                                {
                                    customValues[selectedIndex - newZero] = customValues[selectedIndex - newZero].Substring(0, customValues[selectedIndex - newZero].Length - 1);
                                }

                                PrintMenuItem(selectedIndex, true, customValues[selectedIndex - newZero]);
                                key = Console.ReadKey(true);
                            }
                            
                            // This covers most edge cases of stupid user input. One comes to mind that remains, though. If the user inputs something like 0000000000001, this code will think it is above the integer maximum, and adjust accordingly. This is stupid of course, but the code to patch this edge case is too much effort for how niche it is.
                            int gridSizeXMax = (Console.LargestWindowWidth - 6) / 2;
                            int gridSizeYMax = (Console.LargestWindowHeight - 2) / 2;
                            gridSizeX = customValues[0].Length == 0 ? difficulties[0].gridSizeX : customValues[0].Length > int.MaxValue.ToString().Length - 1 ? gridSizeXMax : Math.Min(int.Parse(customValues[0]), gridSizeXMax);
                            gridSizeY = customValues[1].Length == 0 ? difficulties[0].gridSizeY : customValues[1].Length > int.MaxValue.ToString().Length - 1 ? gridSizeYMax : Math.Min(int.Parse(customValues[1]), gridSizeYMax);
                            bombs = customValues[2].Length == 0 ? difficulties[0].bombs : customValues[2].Length > int.MaxValue.ToString().Length - 1 ? gridSizeX * gridSizeY - 1 : Math.Min(int.Parse(customValues[2]), gridSizeX * gridSizeY - 1);
                        }
                        if(key.Key != ConsoleKey.Escape)
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
                        else if(key.Key == ConsoleKey.A || key.Key == ConsoleKey.LeftArrow)
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
                else if(menu == 2)
                {
                    return false;
                }
            }
        }

        static void PrintMenuItem(int type, bool selected = false, string customText = "")
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
            else if(type == 1)
            { 
                Console.SetCursorPosition(0, 5); // Settings
                Console.Write(" Settings");
            }
            else if(type == 2)
            {
                Console.SetCursorPosition(0, 7); // Quit
                Console.Write(" Quit");
            }
            else if (type == 3)
            {
                Console.SetCursorPosition(0, difficulties.Length * 2 + 3); // Custom difficulty
                Console.Write(" Custom      ");
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

        static void Minesweeper()
        {
            // Set variables
            cursor = (0, 0);
            bombsMinusFlags = bombs;
            grid = new char[gridSizeX, gridSizeY];
            isRevealed = new bool[gridSizeX, gridSizeY];
            flagPlaced = new bool[gridSizeX, gridSizeY];
            timeSinceGameStart = 0;
            currentSmiley = ":)";

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
                ConsoleKeyInfo key = Console.ReadKey(true);
                mre1.Reset(); // Since pretty much the entire game happens in this loop, stopping the timer thread while the code in this loop runs *should* make the game thread-safe, but idk for sure lol
                if (key.Key == ConsoleKey.Escape)
                {
                    gridGenerated = false;
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
                if(!playerDied && !PlayerWon())
                {
                    if(random.Next(1) == 0)
                    {
                        PrintHeadline("", true);
                    }
                }
                mre1.Set();
                Thread.Sleep(2);
                mre2.WaitOne();
            }
            if (playerDied)
            {
                PrintHeadline("X(");
                Thread.Sleep(250);
                // Reveal the whole grid
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for(int j = 0; j < grid.GetLength(1); j++)
                    {
                        if(!isRevealed[i,j]) RevealSpot(i, j);
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
            gridGenerated = false;
            timerIsRunning = false;
        }

        static void PrintHeadline(string smiley = ":)", bool changeSmiley = false) // If newSmiley is empty, I will consider that wanting to make it random
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = headlineColor;
            Console.ForegroundColor = (int)headlineColor < 9 ? ConsoleColor.White : ConsoleColor.Black;
            if(smiley.Length == 0)
            {
                // Choose random fun smiley
                string[] smileys = new string[]
                {
                    ":/", ":P", "xD", ":I", ";)", ":o", ":>", "x)", ":3", "(:", " :)", ":) ", ":-)", ">:)", ">xP", ">:O", ":U", ":T", ":c", "c:", ":3c", "B)", "OwO", "O-o", ">-O",":x", ";-;"
                };
                smiley = smileys[random.Next(smileys.Length)];
                currentSmiley = smiley;
            }
            int bombCounterSize = bombsMinusFlags.ToString().Length;
            bombCounterSize = bombCounterSize <= gridSizeX * 2 ? bombCounterSize : gridSizeX * 2;
            bombCounterSize = Math.Max(gridSizeX > 1 ? 3 : 2, bombCounterSize);
            bool displayTimer = gridSizeX * 2 - bombCounterSize > 3;
            bool displaySmiley = gridSizeX * 2 - bombCounterSize > 3 + (changeSmiley ? smiley.Length + 1 : currentSmiley.Length + 1);
            int padSize = Console.WindowWidth - bombCounterSize - (displayTimer ? 3 : 0) - (displaySmiley ? smiley.Length : 0);
            string pad = new string(' ', padSize / 2); // Padding between stuff in the menu bar of the game. -3 and -2 because of the smiley and timer.
            int localTime = timeSinceGameStart;
            Console.Write($"\x1B[4m{(bombsMinusFlags.ToString().Length > bombCounterSize ? new string('9', bombCounterSize) : bombsMinusFlags.ToString()).PadLeft(bombCounterSize, '0') + pad + (displaySmiley ? changeSmiley ? smiley : currentSmiley : "") + (padSize % 2 != 0 ? " " : "") + pad + (displayTimer ? localTime > 999 ? "999" : localTime.ToString().PadLeft(3,'0') : "")}\x1B[0m");
            if (!gridGenerated) Console.Write("\n");
        }

        static void PrintEmptyGrid()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            for(int i = 0; i <  grid.GetLength(0); i++)
            {
                for(int j = 0;  j < grid.GetLength(1); j++)
                {
                    PrintChar(i, j, i == cursor.x && j == cursor.y);
                }
                if (i != grid.GetLength(0) - 1) Console.Write("\n");
            }
        }
        static void PrintChar(int x, int y, bool isSelected = false)
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

        static void PopulateGrid(int bombs) // Put an amount of bombs specified by the bombs parameter randomly onto the field.
        {
            // Bombs will be distributed via a ticket system
            // Each index of the grid will draw a random number from the ticket array, which will then be removed from the array
            // When this is done, all numbers below a value specified by the bombs parameter will be made into bombs.

            int[] tickets = new int[grid.Length - 1]; // Create a ticket per grid slot, minus one for where the cursor is at the beginning of the game
            int[,] gridProxy = new int[grid.GetLength(0), grid.GetLength(1)]; // A stand-in for the grid array, with ints instead of chars

            // Create the tickets
            for(int i = 0;  i < tickets.Length; i++)
            {
                tickets[i] = i;
            }

            // Assign the tickets
            for(int i = 0; i < gridProxy.GetLength(0); i++)
            {
                for(int j = 0; j < gridProxy.GetLength(1); j++)
                {
                    // Skip this spot and assign the max value (to assure no bomb is spawned) if this is where the cursor is currently at.
                    if(i == cursor.x && j == cursor.y)
                    {
                        gridProxy[i, j] = gridProxy.Length - 1;
                        continue;
                    }

                    // Draw a random ticket and assign it in gridProxy
                    int randomIndex = random.Next(0, tickets.Length);
                    gridProxy[i, j] = tickets[randomIndex];

                    // Remove the drawn ticket from the tickets array (This functions similarly to List.RemoveAt()).
                    int[] updatedTickets = new int[tickets.Length - 1];
                    for(int k = 0; k < tickets.Length; k++)
                    {
                        if (k == randomIndex) continue;
                        updatedTickets[k < randomIndex ? k : k - 1] = tickets[k];
                    }
                    tickets = updatedTickets;
                }
            }
            // Insert the bombs into the grid at the chosen spots
            for(int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    // If the drawn ticket number is less than the number of bombs to place, place a bomb in that position, otherwise place an empty space
                    if (gridProxy[i, j] < bombs) grid[i, j] = bombChar;
                    else                         grid[i, j] = ' ';
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

                    if (x != 0                     && y != 0                     && grid[x - 1, y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 0 (-1,-1)
                    if (                              y != 0                     && grid[x,     y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 1 ( 0,-1)
                    if (x != grid.GetLength(0) - 1 && y != 0                     && grid[x + 1, y - 1] == bombChar) numberOfAdjacentBombs++; // Check pos 2 ( 1,-1)

                    if (x != 0                                                   && grid[x - 1, y    ] == bombChar) numberOfAdjacentBombs++; // Check pos 3 (-1, 0)
                    if (x != grid.GetLength(0) - 1                               && grid[x + 1, y    ] == bombChar) numberOfAdjacentBombs++; // Check pos 4 (+1, 0)

                    if (x != 0                     && y != grid.GetLength(1) - 1 && grid[x - 1, y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 5 (-1, 1)
                    if (                              y != grid.GetLength(1) - 1 && grid[x,     y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 6 ( 0, 1)
                    if (x != grid.GetLength(0) - 1 && y != grid.GetLength(1) - 1 && grid[x + 1, y + 1] == bombChar) numberOfAdjacentBombs++; // Check pos 7 ( 1, 1)
                    
                    grid[x, y] = (char)(numberOfAdjacentBombs + 48); // Have to add 48 to convert the int to the proper ascii value before casting to char. Lookup table for reference: https://www.ascii-code.com/
                }
            }
        }
        static void RevealSpot(int x, int y, bool calledRecursively = false)
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
                timerIsRunning = true;
                new Thread(() =>
                {
                    while (timerIsRunning)
                    {
                        Thread.Sleep(998);
                        timeSinceGameStart++;
                        mre1.WaitOne();
                        Thread.Sleep(2);
                        mre2.Reset();
                        if (!playerDied && !PlayerWon()) PrintHeadline(currentSmiley);
                        mre2.Set();
                    }
                    
                    mre2.Set();
                }).Start();
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

            for(int i = Math.Max(0, x - 1); i < Math.Min(grid.GetLength(0), x + 2); i++)
            {
                for (int j = Math.Max(0, y - 1); j < Math.Min(grid.GetLength(1), y + 2); j++)
                {
                    RevealSpot(i, j, true);
                }
            }
        }

        static void ChangeFlag(int x, int y) // Place or remove flag
        {
            // Do nothing if the spot is revealed
            if (isRevealed[x, y]) return;

            if (flagPlaced[x, y]) bombsMinusFlags++;
            else bombsMinusFlags--;
            flagPlaced[x, y] = !flagPlaced[x, y];

            PrintHeadline();
            PrintChar(x, y, true);
        }

        static bool PlayerWon() // Check if the player has won
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
    }
}