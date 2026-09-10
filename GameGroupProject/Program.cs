namespace GameGroupProject
{
    internal class Program
    {
        static int gridSizeX;
        static int gridSizeY;
        static char[,] grid = new char[gridSizeX, gridSizeY];
        static bool[,] isRevealed = new bool[gridSizeX, gridSizeY];
        static void Main(string[] args)
        {
            
            Console.Write("Grid size x: ");
            while (!int.TryParse(Console.ReadLine(), out gridSizeX)) ;
            Console.Write("Grid size y: ");
            while (!int.TryParse(Console.ReadLine(), out gridSizeY)) ;
            grid = new char[gridSizeX, gridSizeY];
            isRevealed = new bool[gridSizeX, gridSizeY];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for(int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = (char)new Random().Next(0, 9);
                    isRevealed[i, j] = true;
                    //PrintChar(i, j);
                    Console.Write(new Random().Next(0, 9));
                }
                Console.WriteLine("d");
            }    
        }

        static void PrintChar(int x, int y)
        {
            // Print flag or empty square if not revealed.
            if (!isRevealed[x, y])
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Gray;
                if (grid[x,y] == 'P')
                {
                    Console.Write('P');
                }
                else
                {
                    Console.Write(' ');
                }
                Console.ResetColor();
                return;
            }

            // Decide on colour
            Console.BackgroundColor = ConsoleColor.Black;
            switch (grid[x, y])
            {
                default: // Case 0
                    Console.ForegroundColor = ConsoleColor.Black;
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
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            // Print character
            Console.Write(grid[x, y]);
            Console.ResetColor();
        }
    }
}
