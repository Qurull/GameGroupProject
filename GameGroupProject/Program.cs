namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            PlayJeopardy();

            // exit program
            Console.WriteLine("\nPress any key to exit program.");
            Console.ReadKey();
        }

        static void PlayJeopardy()
        {
            // array of categories
            string[] categories = { "Category1", "Category2", "Category3", "Category4", "Category5", "Category6" };

            // array of clues
            string[,] clues =
            {
                {"Clue00", "Clue01", "Clue02", "Clue03", "Clue04" },
                {"Clue10", "Clue11", "Clue12", "Clue13", "Clue14" },
                {"Clue20", "Clue21", "Clue22", "Clue23", "Clue24" },
                {"Clue30", "Clue31", "Clue32", "Clue33", "Clue34" },
                {"Clue40", "Clue41", "Clue42", "Clue43", "Clue44" },
                {"Clue50", "Clue51", "Clue52", "Clue53", "Clue54" }
            };

            // array of answers
            string[,] answers =
            {
                {"Answer00", "Answer01", "Answer02", "Answer03", "Answer04" },
                {"Answer10", "Answer11", "Answer12", "Answer13", "Answer14" },
                {"Answer20", "Answer21", "Answer22", "Answer23", "Answer24" },
                {"Answer30", "Answer31", "Answer32", "Answer33", "Answer34" },
                {"Answer40", "Answer41", "Answer42", "Answer43", "Answer44" },
                {"Answer50", "Answer51", "Answer52", "Answer53", "Answer54" }
            };

            // the points player1 has earned
            int player1Points = 0;

            // jeopardy game loop
            while (true)
            {
                Console.Clear();

                // check if there are any clues left - if not break the while loop
                bool cluesLeft = true;

                foreach (string clue in clues)
                {
                    if (clue != "")
                    {
                        cluesLeft = true;

                        break;
                    }
                    else cluesLeft = false;
                } //end foreach loop

                if (!cluesLeft) break;

                // write out board
                // points
                Console.WriteLine("Player1: " + player1Points + " pts");

                Console.WriteLine("\n");

                // categories
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.Write(categories[i] + " ");
                } //end for loop

                Console.WriteLine("\n");

                // clues/the points hiding each clue
                for (int col = 0; col < clues.GetLength(1); col++)
                {
                    for (int row = 0; row < clues.GetLength(0); row++)
                    {
                        // if the clue has been picked before: change the text color to indicate that it cannot be chosen again
                        if (clues[row, col] == "") Console.ForegroundColor = ConsoleColor.DarkGray;

                        // write out the points for each clue
                        Console.Write((col + 1) * 2 + "00 ");

                        // reset the text color
                        Console.ResetColor();
                    }

                    Console.WriteLine("\n");
                } //end for loop

                // choose category and clue
                Console.WriteLine("Please choose a category.");
                Console.WriteLine("Write the category name or number.");

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

                // if the categories did not match, check if categories match number
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

                Console.WriteLine("Chosen category: " + categories[categoryNumber]);

                // choose clue
                Console.WriteLine("Please choose which clue you want.");
                Console.WriteLine("Write the point-amount of the clue you want.");

                string chosenClue = Console.ReadLine() ?? "";

                if (int.TryParse(chosenClue, out int clueNumber))
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
                else
                {
                    InvalidInput("clue");
                    continue;
                }

                // check if clue has been taken
                if (clues[categoryNumber, clueNumber] == "")
                {
                    InvalidInput("clue");
                    continue;
                }

                Console.Clear();

                // display clue and ask for an answer
                Console.WriteLine(categories[categoryNumber] + " for " + chosenClue);
                Console.WriteLine(clues[categoryNumber, clueNumber]);

                Console.WriteLine("Please write answer.");
                Console.WriteLine("Remember to include 'What is' in your answer.");

                // check if answer is correct
                string answerInput = Console.ReadLine() ?? "";

                bool correctAnswer = false;

                if (answerInput.ToUpper().Trim() == answers[categoryNumber, clueNumber].ToUpper())
                {
                    correctAnswer = true;
                }

                // inform player of correct answer and if their answer is correct
                Console.Clear();

                Console.WriteLine("The correct answer is: " + answers[categoryNumber, clueNumber] + ".");

                Console.WriteLine("Your answer is " + (correctAnswer ? "CORRECT" : "WRONG") + "!");

                // add points
                if (correctAnswer)
                {
                    Console.WriteLine("+ " + chosenClue);

                    player1Points += int.Parse(chosenClue);
                }
                else
                {
                    Console.WriteLine("- " + chosenClue);

                    player1Points -= int.Parse(chosenClue);
                }

                // change clue to be able to gray it out and be unchooseable
                clues[categoryNumber, clueNumber] = "";

                // continue/end game
                Console.WriteLine("Press any key to continue.");

                Console.ReadKey();



            } //end while loop


            static void InvalidInput(string input = "input")
            {
                Console.WriteLine("Invalid " + input + ".");
                Console.WriteLine("Press any key to continue.");

                Console.ReadKey();
            } //end InvalidInput method
        }
    }
}
