namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            JeopardyMenu();

            // exit program
            Console.WriteLine("\nPress any key to exit program.");
            Console.ReadKey();
        }

        static void JeopardyMenu()
        {
            // jeopardy menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Jeopardy!");
                Console.WriteLine("To play the game: press 1 and then enter.");
                Console.WriteLine("To read the rules: press 2 and then enter.");
                Console.WriteLine("To exit the game: press 3 and then enter.");

                string menuChoice = Console.ReadLine() ?? "";

                // start game
                if (menuChoice == "1")
                {
                    ManageRounds();
                }

                // read rules
                else if (menuChoice == "2")
                {
                    Console.Clear();

                    // write out rules
                    Console.WriteLine("Jeopardy rules: ");

                    Console.Write("You can play the game with 1-3 players. \n\n" +
                        "There is a board with columns of categories. Under the categories there are displayed different amounts of points that are hiding clues to the right question you need to ask. \n\n" +
                        "A player is asked to choose category and clue. Then the clue is displayed and all players get the chance to buzz in with their buzz key. \n\n" +
                        "Each player gets assigned a buzz key at the start of the game: \n" +
                        "Player 1 has the A key, \n" +
                        "Player 2 has the L key, \n" +
                        "and Player 3 has the G key. \n\n" +
                        "If you are the fastest to buzz, then you have to answer the clue - and it's very important that you write it out as a question (what is, who is, where is, etc). \n\n" +
                        "If you don't write it as a question or your answer is wrong or misspelled, you will lose the amount of points the clue was worth. After a wrong answer the other players get another chance to answer. \n\n" +
                        "If your answer is correct, spelled right and written as a question, you will gain the amount of points the clue was worth. \n\n" +
                        "A clue can only be chosen once. \n\n" +
                        "The winner is the player who ends the game with the most points. \n \n \n");

                    // go back to menu
                    Console.WriteLine("Press any key to go back to the menu.");
                    Console.ReadKey();
                    continue;
                }

                // exit game
                else if (menuChoice == "3")
                {
                    break;
                }
            }
        } //end JeopardyMenu method

        static void ManageRounds()
        {
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

            // start the round
            PlayJeopardy(playerName, playerColor, playerBuzzer, ref playerAllowed, ref playerPoints, amountOfPlayers);

            // ask if they want to continue with final jeopardy round
            Console.WriteLine("Do you want to continue with the Final Jeopardy round?");
            Console.WriteLine("If no: press tab.");
            Console.WriteLine("If yes: press any other key.");

            if (Console.ReadKey().Key != ConsoleKey.Tab)
            {
                // start final jeopardy
                PlayFinalJeopardy(playerName, playerColor, ref playerAllowed, ref playerPoints);
            }

            // display who won the game
            ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);

            int winner = MostPoints(playerPoints);

            if (winner == -1)
            {
                Console.WriteLine("\n\nCongratulation! It's a tie!");
            }
            else
            {
                Console.Write("\n\nCongratulations, ");
                Console.ForegroundColor = playerColor[winner];
                Console.Write(playerName[winner]);
                Console.ResetColor();
                Console.WriteLine("! You won!");
            }

            Console.WriteLine("Thank you for playing Jeopardy!");

            // go back to jeopardy menu
            Console.WriteLine("Press any key to return to the Jeopardy menu.");
            Console.ReadKey();

        } //end ManageRounds method

        static void PlayJeopardy(string[] playerName, ConsoleColor[] playerColor, ConsoleKey[] playerBuzzer, ref bool[] playerAllowed, ref int[] playerPoints, int amountOfPlayers)
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
                ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);
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


                    // check if answer is correct
                    string answerInput = WriteAnswer(playerName[fastestBuzz], playerColor[fastestBuzz]);

                    bool correctAnswer = CorrectAnswer(answerInput, answers[categoryNumber, clueNumber]);



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

                Console.WriteLine("If you want to end the round, press tab.");
                Console.WriteLine("Press any other key to continue.");

                if (Console.ReadKey().Key == ConsoleKey.Tab)
                {
                    break;
                }
            } //end while loop





        } //end PlayJeopardy method

        static void PlayFinalJeopardy(string[] playerName, ConsoleColor[] playerColor, ref bool[] playerAllowed, ref int[] playerPoints)
        {
            Console.Clear();

            // check if players have more than 0 points - use playerAllowed maybe to know which players can join in the final round
            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerPoints[i] > 0)
                {
                    playerAllowed[i] = true;
                }
                else playerAllowed[i] = false;
            }

            Console.WriteLine("The following players qualify for the Final Jeopardy round: ");

            int playersLeft = 0;

            for (int i = 0; i < playerAllowed.Length; i++)
            {
                if (playerAllowed[i])
                {
                    playersLeft += 1;
                    Console.ForegroundColor = playerColor[i];
                    Console.WriteLine(playerName[i]);
                    Console.ResetColor();
                }
            }

            if (playersLeft > 0)
            {
                // the category is revealed
                // multidimensional array: row 1 holds the category, row 2 holds the clue, and row 3 holds the answer
                string[,] clues = { { "Category1", "Category2", "Category3" }, { "Clue1", "Clue2", "Clue3" }, { "Answer1", "Answer2", "Answer3" } };

                Random random = new Random();

                int finalClue = random.Next(0, clues.GetLength(1));

                int[] playerWagers = { 0, 0, 0 };

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        Console.WriteLine("The final category is " + clues[0, finalClue] + ".");

                        Console.Write("Everyone, except ");
                        Console.ForegroundColor = playerColor[i];
                        Console.Write(playerName[i]);
                        Console.ResetColor();
                        Console.Write(", should look away now.");

                        Console.WriteLine(playerName[i] + ", press any key when you are ready to make a wager.");
                        Console.ReadKey();

                        // wagepoints method here
                        playerWagers[i] = WagePoints(playerName[i], playerColor[i], playerPoints[i]);

                        Console.WriteLine("You have wagered " + playerWagers[i] + " points." +
                            "\nPress any key to continue.");

                        Console.ReadKey();

                        Console.Clear();
                    }
                }

                // array of the player's answers
                string[] playerAnswers = { "", "", "" };

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {

                        // the clue is revealed
                        Console.WriteLine("The final clue is: \n" + clues[1, finalClue]);

                        // every player is asked to write their answer - while the other avert their eyes

                        Console.Write("Everyone, except ");
                        Console.ForegroundColor = playerColor[i];
                        Console.Write(playerName[i]);
                        Console.ResetColor();
                        Console.Write(", should look away now.");

                        Console.WriteLine(playerName[i] + ", press any key when you are ready to write your answer.");
                        Console.ReadKey();

                        playerAnswers[i] = WriteAnswer(playerName[i], playerColor[i]);

                        Console.WriteLine("You have answeredd " + playerAnswers[i] + "." +
                            "\nPress any key to continue.");

                        Console.ReadKey();

                        Console.Clear();

                    }
                }

                // all the answers are revealed one by one and whether the answer is correct

                Console.WriteLine("Now the answers will be revealed: \n");

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        Console.WriteLine("Press any key to reveal the next answer.");
                        Console.ReadKey();

                        Console.ForegroundColor = playerColor[i];
                        Console.Write(playerName[i]);
                        Console.ResetColor();
                        Console.Write(": " + playerAnswers[i] + ".");

                        if (CorrectAnswer(playerAnswers[i], clues[2, finalClue]))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t CORRECT");
                            Console.ResetColor();

                            playerPoints[i] += playerWagers[i];
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\t INCORRECT");
                            Console.ResetColor();

                            playerPoints[i] -= playerWagers[i];
                        }
                    }
                }

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();

                Console.Clear();

                // reveal how many points each player wagered on the final clue
                Console.WriteLine("Points wagered: ");

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        Console.ForegroundColor = playerColor[i];
                        Console.Write(playerName[i]);
                        Console.ResetColor();

                        Console.WriteLine(" wagered " + playerWagers[i] + " of their points.");
                    }
                }


            }
            else
            {
                Console.WriteLine("No player qualifies.");
            }
            // end final jeopardy
            Console.WriteLine("Press any key to end the game and see the results.");

            Console.ReadKey();
        } //end PlayFinalJeopardy method

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

        static string WriteAnswer(string playerName, ConsoleColor playerColor)
        {
            Console.ForegroundColor = playerColor;
            Console.Write(playerName);
            Console.ResetColor();
            Console.WriteLine(", please write answer.");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Remember to form your answer as a question (what is/who is/etc).");
            Console.ResetColor();

            return Console.ReadLine() ?? "";
        } //end Writeanswer method

        static bool CorrectAnswer(string answerInput, string correctAnswer)
        {
            if (answerInput.ToUpper().Trim() == correctAnswer.ToUpper())
            {
                return true;
            }
            else return false;
        } //end CorrectAnswer method

        static void ShowPoints(ConsoleColor[] playerColor, string[] playerName, int[] playerPoints, int amountOfPlayers)
        {
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
        } // end show points method

        static int WagePoints(string playerName, ConsoleColor playerColor, int playerPoints)
        {
            Console.ForegroundColor = playerColor;
            Console.Write(playerName);
            Console.ResetColor();

            Console.WriteLine(", you have earned " + playerPoints + " points.");

            int maxWager = playerPoints;
            Console.WriteLine("You can wager between 1 and " + maxWager + ".");

            Console.WriteLine("If your answer is correct, you will add the amount of points you wagered to your current score. \nIf your answer is incorrect, you will lose the amount of points you wagered.");

            while (true)
            {
                Console.WriteLine("Please input the amount of points you would like to wager.");

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
        } //end WagePoints method


        static int MostPoints(int[] playerPoints)
        {
            int playerWithMostPoints = 0;

            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerWithMostPoints == i || playerPoints[playerWithMostPoints] < playerPoints[i])
                {
                    playerWithMostPoints = i;
                }
            }

            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerPoints[playerWithMostPoints] == playerPoints[i] && playerWithMostPoints != i)
                {
                    playerWithMostPoints = -1;
                    break;
                }
            }

            return playerWithMostPoints;
        } // end most points method
    }
}
