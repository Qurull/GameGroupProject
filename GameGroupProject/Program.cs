namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            PlayJeopardy();

            // exit program
            Console.WriteLine("\nPress any key to exit program.");
            Console.ReadKey();
        }

        static void PlayJeopardy()
        {
            // quiz content

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
            // end quiz content

            // player information

            // each player's name is held in the playerName-array
            string[] playerName = { "Player 1", "Player 2", "Player 3" };
            // each player's color is held in the playerColor-array
            ConsoleColor[] playerColor = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Blue };
            // each player's buzzer-key is held in the playerBuzzer-array
            ConsoleKey[] playerBuzzer = { ConsoleKey.A, ConsoleKey.L, ConsoleKey.G };
            // whether a specific player is allowed to buzz in is held in the playerAllowed-array
            bool[] playerAllowed = { false, false, false };
            // each player's points are held in the playerPoints-array
            int[] playerPoints = { 0, 0, 0 };
            // end player information

            // game setup

            // ask for the amount of players until getting a valid input

            // the amount players playing the game
            int amountOfPlayers = 0;

            while (amountOfPlayers == 0)
            {
                Console.WriteLine("How many will be playing? (1-3)");

                if (int.TryParse(Console.ReadLine(), out int amount))
                {
                    if (amount > 0 && amount < 4) amountOfPlayers = amount;
                }
            } //end while loop

            // ask players for name

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

            // decide who goes first

            // holds which player is next to choose category and clue
            int clueChooser;

            if (amountOfPlayers > 1) clueChooser = BuzzIn(playerName, playerBuzzer, playerAllowed, true);
            else clueChooser = 0;
            // end game setup


            // jeopardy game loop
            while (true)
            {
                Console.Clear();

                // check if there are any clues left - if not break the while loop

                // there are clues left that haven't been picked
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

                playerAllowed[0] = true;

                if (amountOfPlayers > 1)
                {
                    playerAllowed[1] = true;

                    if (amountOfPlayers > 2)
                    {
                        playerAllowed[2] = true;
                    }
                }

                // write out board

                // points
                Console.ForegroundColor = playerColor[0];
                Console.Write(playerName[0] + " " + playerPoints[0] + " pts");

                if (amountOfPlayers > 1)
                {
                    Console.ForegroundColor = playerColor[1];
                    Console.Write("\t" + playerName[1] + " " + playerPoints[1] + " pts");
                    if (amountOfPlayers > 2)
                    {
                        Console.ForegroundColor = playerColor[2];
                        Console.Write("\t" + playerName[2] + " " + playerPoints[2] + " pts");
                    }
                }
                // end points

                Console.ResetColor();

                Console.WriteLine("\n\n");

                // categories
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.Write(categories[i] + " ");
                }
                // end categories

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
                }
                // end clues/points that are hiding clues


                // choose category and clue

                // which player
                Console.ForegroundColor = playerColor[clueChooser];
                Console.Write(playerName[clueChooser]);
                Console.ResetColor();

                // choose category
                Console.WriteLine(", please choose a category.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Write the category name or number.");
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

                // which player
                Console.ForegroundColor = playerColor[clueChooser];
                Console.Write(playerName[clueChooser]);
                Console.ResetColor();

                // choose clue
                Console.WriteLine(", please choose the clue you want.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Write the point-amount of the clue you want.");
                Console.ResetColor();

                string chosenClue = Console.ReadLine() ?? "";

                // check if input is valid

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
                // end choose category and clue

                Console.Clear();

                // display clue and make player answer
                while (true)
                {
                    int buzzersLeft = BuzzersLeft(playerAllowed);

                    if (buzzersLeft < 1)
                    {
                        Console.Clear();

                        Console.WriteLine("Clue: ");
                        Console.WriteLine(clues[categoryNumber, clueNumber]);
                        Console.WriteLine("\n\nThe correct answer is: ");
                        Console.WriteLine(answers[categoryNumber, clueNumber]);

                        Console.WriteLine("\n\nPress any key to continue.");
                        Console.ReadKey();
                        break;
                    }

                    // display clue and ask for an answer
                    Console.WriteLine(categories[categoryNumber] + " for " + chosenClue);
                    Console.WriteLine(clues[categoryNumber, clueNumber]);

                    int fastestBuzz;

                    if (buzzersLeft >= 2)
                    {
                        fastestBuzz = BuzzIn(playerName, playerBuzzer, playerAllowed);
                    }
                    else
                    {
                        fastestBuzz = LastBuzzer(playerAllowed);

                        if (amountOfPlayers > 1)
                        {
                            Console.ForegroundColor = playerColor[fastestBuzz];
                            Console.Write(playerName[fastestBuzz]);
                            Console.ResetColor();
                            Console.WriteLine(", do you want to answer?");

                            Console.WriteLine("Press tab if no, press any other key if yes.");

                            if (Console.ReadKey().Key == ConsoleKey.Tab)
                            {
                                fastestBuzz = -1;
                            }
                        }
                    }

                    if (fastestBuzz == -1)
                    {
                        break;
                    }

                    Console.ForegroundColor = playerColor[fastestBuzz];
                    Console.Write(playerName[fastestBuzz]);
                    Console.ResetColor();
                    Console.WriteLine(", please write answer.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Remember to form your answer as a question (what is/who is/etc).");
                    Console.ResetColor();

                    // check if answer is correct
                    string answerInput = Console.ReadLine() ?? "";

                    bool correctAnswer = false;

                    if (answerInput.ToUpper().Trim() == answers[categoryNumber, clueNumber].ToUpper())
                    {
                        correctAnswer = true;
                    }

                    // inform player of correct answer and if their answer is correct
                    Console.Clear();

                    Console.WriteLine("Your answer was: " + answerInput + ".");

                    Console.WriteLine("Your answer is " + (correctAnswer ? "CORRECT" : "WRONG") + "!");

                    // add points
                    if (correctAnswer)
                    {
                        Console.WriteLine("+ " + chosenClue);

                        playerPoints[fastestBuzz] += int.Parse(chosenClue);

                        clueChooser = fastestBuzz;

                        break;
                    }
                    else
                    {
                        Console.WriteLine("- " + chosenClue);

                        playerPoints[fastestBuzz] -= int.Parse(chosenClue);

                        playerAllowed[fastestBuzz] = false;
                    }
                }


                // change clue to be able to gray it out and be unchooseable
                clues[categoryNumber, clueNumber] = "";

                // continue/end game

                Console.WriteLine("If you want to end the game, press tab.");
                Console.WriteLine("Press any other key to continue.");

                if (Console.ReadKey().Key == ConsoleKey.Tab)
                {
                    break;
                }
            } //end while loop





        } //end PlayJeopardy method

        static void InvalidInput(string input = "input")
        {
            Console.WriteLine("Invalid " + input + ".");
            Console.WriteLine("Press any key to continue.");

            Console.ReadKey();
        } //end InvalidInput method

        /// <summary>
        /// in their player color:
        /// tells the player which key their buzzer is,
        /// asks the player to write their name
        /// </summary>
        /// <param name="player">playerName, used for addressing the player and for changing the player's name</param>
        /// <param name="buzzer">playerBuzzer, used to tell the player which key their buzzer is</param>
        /// <param name="color">playerColor, used to color the text</param>
        static void EnterName(ref string player, ConsoleKey buzzer, ConsoleColor color)
        {
            while (true)
            {
                Console.ForegroundColor = color;

                Console.WriteLine(player + " will have the " + buzzer + "-key as their buzzer.");
                Console.WriteLine("Please write the name of " + player + ".");

                Console.ResetColor();

                string name = Console.ReadLine() ?? "";

                if (name != "")
                {
                    player = name;
                    break;
                }
                else InvalidInput();
            }
        } //end EnterName method

        // check how many players who are allowed to buzz in are left
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

        // check which player is the last player who is allowed to buzz in
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

        static int BuzzIn(
            string[] playerName,
            ConsoleKey[] playerBuzzer,
            bool[] playerAllowed,
            bool clueChoice = false
            )
        {
            int buzzWinner;

            Console.WriteLine("Get ready to buzz in!");

            Console.WriteLine("Remember your buzz-keys: ");

            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerAllowed[i]) Console.WriteLine(playerName[i] + ": " + playerBuzzer[i]);
            }

            Console.WriteLine("You can buzz in after the beep (when 'BUZZ IN' is written in green letters).");

            Console.WriteLine("Press any key, when you are all ready to buzz in, to continue.");

            Console.ReadKey();

            Console.Beep(700, 3000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("BUZZ IN");
            Console.ResetColor();

            if (!clueChoice)
            {
                Console.WriteLine("If none of you know the answer, you can skip answering by pressing tab.");
            }

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

        } //end BuzzIn method
    }
}
