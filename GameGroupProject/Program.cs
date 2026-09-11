namespace GameGroupProject
{

    // jeopardy category
    enum Category { Category1, Category2, Category3, Category4, Category5, Category6 }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // jeopardy clues
            string[,] clues =
            {
                {"Clue00", "Clue01", "Clue02", "Clue03", "Clue04" },
                {"Clue10", "Clue11", "Clue12", "Clue13", "Clue14" },
                {"Clue20", "Clue21", "Clue22", "Clue23", "Clue24" },
                {"Clue30", "Clue31", "Clue32", "Clue33", "Clue34" },
                {"Clue40", "Clue41", "Clue42", "Clue43", "Clue44" },
                {"Clue50", "Clue51", "Clue52", "Clue53", "Clue54" }
            };

            // jeopardy answers
            string[,] answers =
            {
                {"Answer00", "Answer01", "Answer02", "Answer03", "Answer04" },
                {"Answer10", "Answer11", "Answer12", "Answer13", "Answer14" },
                {"Answer20", "Answer21", "Answer22", "Answer23", "Answer24" },
                {"Answer30", "Answer31", "Answer32", "Answer33", "Answer34" },
                {"Answer40", "Answer41", "Answer42", "Answer43", "Answer44" },
                {"Answer50", "Answer51", "Answer52", "Answer53", "Answer54" }
            };


            // jeopardy grid

            // write out each category - could've used row for the for loop
            for (int i = 0; i < 6; i++)
            {
                Console.Write((Category)i + " ");
            }

            Console.WriteLine("\n");

            // write out grid of clues
            for (int col = 0; col < clues.GetLength(1); col++) // for every clue in a category
            {
                for (int row = 0; row < clues.GetLength(0); row++) // for every category (row)
                {
                    // if the clue has been picked before: change the text color to indicate that it cannot be chosen again
                    if (clues[row, col] == "empty") Console.ForegroundColor = ConsoleColor.DarkGray;

                    // write out the points for each clue
                    Console.Write(clues[row, col] + " " + (col + 1) + "00");

                    // reset the text color
                    Console.ResetColor();
                }

                Console.WriteLine("\n");
            }

            // choose category
            Console.WriteLine("Please choose a category (1-6)");

            Category chosenCategory = (Category)Convert.ToInt32(Console.ReadLine()) - 1;
            // needs to be able to write the category out (for fun)
            // needs to be "safe" so if you input an incorrect value it will tell you to try again

            Console.WriteLine("Chosen category: " + chosenCategory);

            // choose clue
            Console.WriteLine("Please choose which clue you want (1-5)");

            int chosenClue = Convert.ToInt32(Console.ReadLine()) - 1;
            // needs to be able to write the clue's point amount (for fun)
            // needs to be "safe" so if you input incorrect value it will tell you to try again


            // write out chosen clue
            Console.WriteLine("Chosen clue: " + clues[(int)chosenCategory, chosenClue]);

            // change clue to be able to gray it out and be unchooseable
            clues[(int)chosenCategory, chosenClue] = "hello";

            Console.WriteLine("Chosen clue: " + clues[(int)chosenCategory, chosenClue]);

            // exit program
            Console.WriteLine("\nPress any key to exit program.");
            Console.ReadKey();
        }

        // append integer method - might not use it after all
        static int AppendInt(int a, int b)
        {

            return Convert.ToInt32(a.ToString() + b.ToString());

        }
    }
}
