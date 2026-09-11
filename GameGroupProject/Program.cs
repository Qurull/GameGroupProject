using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        const int ROW_SIZE = 5; 
        const int COL_SIZE = 4;

        static readonly Random random = new();

        static readonly char[] pins = ['○', '◔', '◑', '◕', '●'];

        static readonly ConsoleColor[] possibleColors = [
            ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, 
            ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Yellow
        ];

        static readonly ConsoleColor[,] colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];

        static readonly ConsoleColor[] secretColors = new ConsoleColor[COL_SIZE];

        static int selectedRow, selectedCol;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            GenerateSecretColors();
            (int left, int top) = GenerateBoard();
            DisplaySelectColors(left, top);
        }

        static (int BoardLeft, int BoardTop) GenerateBoard()
        {
            Console.WriteLine("===== MasterMind =====");
            (int boardLeft, int boardTop) = Console.GetCursorPosition();

            for (int row = 0; row < colorFields.GetLength(0); row++)
            {
                for (int col = 0; col < colorFields.GetLength(1); col++)
                    Console.Write(pins[0]);

                Console.WriteLine(" | Pins: " + pins[0]);
            }

            for (int i = 0; i < secretColors.Length; i++)
                Console.Write(pins[0]);

            Console.WriteLine(" | ");

            return (boardLeft, boardTop);
        }

        static void GenerateSecretColors()
        {
            for (int i = 0; i < secretColors.Length; i++)
                secretColors[i] = possibleColors[random.Next(possibleColors.Length)];
        }

        static void DisplaySelectColors(int cursorLeft, int cursorTop)
        {
            Console.Write("\nSelect a color: ");

            (int textLeft, int textTop) = Console.GetCursorPosition();
            Console.CursorVisible = false;

            int selectedColorIndex = 0;

            while (selectedRow <= ROW_SIZE)
            {
                DisplayColorOptions(selectedColorIndex, textLeft, textTop);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        selectedColorIndex = int.Max(selectedColorIndex - 1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        selectedColorIndex = int.Min(selectedColorIndex + 1, possibleColors.Length - 1);
                        break;
                    case ConsoleKey.Enter:
                        PlaceColor(possibleColors[selectedColorIndex], cursorLeft, cursorTop);
                        break;
                }

                Console.SetCursorPosition(textLeft, textTop);
            }
        }

        static void DisplayColorOptions(int index, int left, int top)
        {
            for (int i = 0; i < possibleColors.Length; i++)
            {
                Console.SetCursorPosition(left + i, top);

                Console.ForegroundColor = possibleColors[i];
                Console.Write(pins[^1]);
                Console.ResetColor();

                Console.SetCursorPosition(left + i, top + 1);
                Console.Write(index == i ? '▲' : ' ');
            }
        }

        static void PlaceColor(ConsoleColor color, int cursorLeft, int cursorTop)
        {
            Console.SetCursorPosition(selectedCol, cursorTop + selectedRow);

            Console.ForegroundColor = color;
            Console.Write(pins[^1]);
            Console.ResetColor();

            colorFields[selectedRow, selectedCol] = color;
            selectedCol++;

            if (selectedCol == COL_SIZE)
            {
                PlacePins();
                selectedCol = 0;
                selectedRow++;
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        static void PlacePins()
        {
            int correctColorsCount = 0;

            for (int col = 0; col < secretColors.Length; col++)
                if (colorFields[selectedRow, col] == secretColors[col])
                    correctColorsCount++;

            Console.SetCursorPosition(COL_SIZE + 9, selectedRow + 1);
            Console.WriteLine(pins[correctColorsCount]);
        }
    }
}
