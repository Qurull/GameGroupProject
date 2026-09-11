using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        // In order for program to look good, font must be Consolas size 16, bold.
        static (string name, int gridSizeX, int gridSizeY, int bombs)[] t = new (string, int, int, int)[]
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
        static char flagChar = 'F';
        static int gridSizeX;
        static int gridSizeY;
        static int bombs;
        static int bombsMinusFlags;
        static char[,] grid = new char[gridSizeX, gridSizeY];
        static bool[,] isRevealed = new bool[gridSizeX, gridSizeY];
        static bool[,] flagPlaced = new bool[gridSizeX, gridSizeY];
        static Random random = new Random();
        static (int x, int y) cursor = (0,0);
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
            while (true)
            {
                MinesweeperMenu();
                Minesweeper();
            }
        }
        static void MinesweeperMenu()
        {
            Console.SetWindowSize(25, 15);
            Console.Clear();
            Console.WriteLine("\n M I N E S W E E P E R !");
            Console.ReadKey(true);
        }

        static void Minesweeper()
        {
            // Set variables
            cursor = (0, 0);
            gridSizeX = 16;
            gridSizeY = 16;
            bombs = 40;
            bombsMinusFlags = bombs;
            grid = new char[gridSizeX, gridSizeY];
            isRevealed = new bool[gridSizeX, gridSizeY];
            flagPlaced = new bool[gridSizeX, gridSizeY];

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

                if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.Enter)
                {
                    RevealSpot(cursor.x, cursor.y);
                    PrintChar(cursor.x, cursor.y, true);
                    continue;
                }

                if (key.Key == (ConsoleKey)flagChar)
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
            }
            if (playerDied)
            {
                playerDied = false;
                Console.ReadKey(true);
                Console.Clear();
                Console.WriteLine("You died :(");
                Console.ReadKey(true);
            }
            else
            {
                Console.ReadKey(true);
                Console.Clear();
                Console.WriteLine("You won :)");
                Console.ReadKey(true);
            }
            gridGenerated = false;
        }

        static void PrintHeadline()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = headlineColor;
            Console.ForegroundColor = (int)headlineColor < 9 ? ConsoleColor.White : ConsoleColor.Black;
            string smiley = ":)";
            int bombCounterSize = bombsMinusFlags.ToString().Length;
            bombCounterSize = bombCounterSize <= gridSizeX * 2 ? bombCounterSize : gridSizeX * 2;
            bombCounterSize = Math.Max(gridSizeX > 1 ? 3 : 2, bombCounterSize);
            bool displayTimer = gridSizeX * 2 - bombCounterSize > 3;
            bool displaySmiley = gridSizeX * 2 - bombCounterSize > 3 + smiley.Length + 1;
            int padSize = Console.WindowWidth - bombCounterSize - (displayTimer ? 3 : 0) - (displaySmiley ? smiley.Length : 0);
            string pad = new string(' ', padSize / 2); // Padding between stuff in the menu bar of the game. -3 and -2 because of the smiley and timer.
            Console.Write($"\x1B[4m{(bombsMinusFlags.ToString().Length > bombCounterSize ? new string('9', bombCounterSize) : bombsMinusFlags.ToString()).PadLeft(bombCounterSize).Replace(' ', '0') + pad + (displaySmiley ? smiley : "") + (padSize % 2 != 0 ? " " : "") + pad + (displayTimer ? "000" : "")}\x1B[0m");
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
                    Console.Write(flagChar);
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
        static void RevealSpot(int x, int y)
        {
            // If this is the first reveal, populate the grid with bombs. Doing this will guarantee a safe spot on the cursor.
            if (!gridGenerated)
            {
                PopulateGrid(bombs);
                gridGenerated = true;
            }

            // Do nothing if the spot is already revealed
            if (isRevealed[x, y]) return;

            // Do nothing if the player has marked this spot as a bomb
            if (flagPlaced[x, y]) return;

            // Game ends if a bomb is revealed
            if (grid[x, y] == bombChar)
            {
                playerDied = true;
            }

            // Update reveal status and print the character
            isRevealed[x, y] = true;
            PrintChar(x, y);

            // Reveal adjacent spots if there are 0 bombs nearby
            if (!(grid[x, y] == '0')) return;

            for(int i = Math.Max(0, x - 1); i < Math.Min(grid.GetLength(0), x + 2); i++)
            {
                for (int j = Math.Max(0, y - 1); j < Math.Min(grid.GetLength(1), y + 2); j++)
                {
                    RevealSpot(i, j);
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
