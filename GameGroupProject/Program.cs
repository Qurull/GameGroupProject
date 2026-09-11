using System.Text;

namespace GameGroupProject
{
    internal class Program
    {
        static readonly Random random = new();

        const int ROW_SIZE = 5, COL_SIZE = 4;

        static readonly ConsoleColor[] possibleColors = [ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Yellow];

        static readonly ConsoleColor[,] colorFields = new ConsoleColor[ROW_SIZE, COL_SIZE];

        static readonly ConsoleColor[] correctColors = new ConsoleColor[COL_SIZE];

        static readonly char[] chars = ['○', '◔', '◑', '◕', '●'];

        static int selectedRow = 0, selectedCol = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("===== MasterMind =====");
            (int left, int top) = Console.GetCursorPosition();

            for (int i = 0; i < correctColors.Length; i++)
                correctColors[i] = possibleColors[random.Next(possibleColors.Length)];

            for (int i = 0; i < colorFields.GetLength(0); i++)
            {
                for (int j = 0; j < colorFields.GetLength(1); j++)
                    Console.Write(chars[0]);

                Console.Write(" | Pins: " + chars[0]);
                Console.WriteLine();
            }

            for (int i = 0; i < correctColors.Length; i++)
                Console.Write('◌');
            Console.Write(" | ");

            string message = "\n\nSelect a color: ";
            int colorIndex = 0;

            Console.Write(message);
            (int l, int t) = Console.GetCursorPosition();
            Console.CursorVisible = false;

            while (true)
            {
                for (int i = 0; i < possibleColors.Length; i++)
                {
                    Console.ForegroundColor = possibleColors[i];
                    Console.Write(chars[^1]);
                    Console.ResetColor();

                    Console.SetCursorPosition(message.Length - 2 + i, t + 1);
                    Console.Write(colorIndex == i ? '▲' : ' ');
                    Console.SetCursorPosition(message.Length - 1 + i, t);
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        colorIndex = int.Max(colorIndex - 1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        colorIndex = int.Min(colorIndex + 1, possibleColors.Length - 1);
                        break;
                    case ConsoleKey.Enter:
                        PlaceBall(possibleColors[colorIndex], top, left);
                        break;
                }

                Console.SetCursorPosition(l, t);
            }
        }

        static void PlaceBall(ConsoleColor color, int row, int col)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(selectedCol, row + selectedRow);

            Console.ForegroundColor = color;
            Console.Write("●");
            Console.ResetColor();

            Console.SetCursorPosition(left, top);
            colorFields[selectedRow, selectedCol] = color;

            if (selectedCol == COL_SIZE - 1)
            {
                PlacePins(row);
                selectedRow++;
            }
            selectedCol = (selectedCol + 1) % COL_SIZE;

            Console.SetCursorPosition(left, top);
        }

        static void PlacePins(int row)
        {
            int pins = 0;

            for (int i = 0; i < correctColors.Length; i++)
                if (colorFields[selectedRow, i] == correctColors[i])
                    pins++;

            Console.SetCursorPosition(COL_SIZE + 9, row + selectedRow);
            Console.WriteLine(chars[pins]);
        }

        static void PrintFields()
        {
            for (int i = 0; i < colorFields.GetLength(0); i++)
            {
                for (int j = 0; j < colorFields.GetLength(1); j++)
                    Console.Write(colorFields[i, j] + " ");
                Console.WriteLine();
            }
        }
    }
}
